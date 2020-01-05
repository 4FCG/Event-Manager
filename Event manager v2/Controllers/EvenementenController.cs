using Event_manager_v2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Event_manager_v2.Controllers
{
    public class EvenementenController : Controller
    {
        private DataModelContext db = new DataModelContext();

        // GET: Evenementen
        public ActionResult Index()
        {
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            ViewBag.UserId = userId;
            return View(db.Evenements.ToList());
        }

        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "evenement_id,naam,beschrijving,begindatum,einddatum")] Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                db.Evenements.Add(evenement);
                EvenementBeheerder evenementBeheerder = new EvenementBeheerder
                {
                    evenement = evenement.evenement_id,
                    beheerder = Convert.ToInt32(User.Identity.GetUserId())
                };
                db.EvenementBeheerders.Add(evenementBeheerder);
                db.SaveChanges();
                return RedirectToAction("Dashboard", new { id = evenement.evenement_id });
            }
            return View(evenement);
        }

        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evenement evenement = db.Evenements.Find(id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            IEnumerable<Beheerder> beheerders = db.Beheerders.ToList().Where(beheerder => evenement.EvenementBeheerders.Select(m => m.beheerder).Contains(beheerder.beheerder_id));
            if (!beheerders.Select(m => m.beheerder_id).Contains(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            IEnumerable<Deelnemer> deelnemers = db.Deelnemers.ToList().Where(d => d.evenement == evenement.evenement_id);

            EvenementDashboardViewModel viewmodel = new EvenementDashboardViewModel
            {
                Evenement = evenement,
                Activiteiten = evenement.Activiteits,
                Beheerders = beheerders,
                Deelnemers = deelnemers
            };

            return View(viewmodel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evenement evenement = db.Evenements.Find(id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            if (!User.Identity.IsAuthenticated || !IsEventBeheerder(evenement))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(evenement);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "evenement_id,naam,beschrijving,begindatum,einddatum")] Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                if (db.Evenements.Find(evenement.evenement_id).EvenementBeheerders.Count() > 1)
                {
                    TempData["wijziging"] = GenerateWijziging(evenement, 3);
                    return RedirectToAction("Create", "Wijzigingen", null);
                }
                else
                {
                    db.Entry(evenement).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", new { id = evenement.evenement_id });
                }

            }
            return View(evenement);
        }

        //Wijziging delete redirects to dashboard on execution which results in an error

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evenement evenement = db.Evenements.Find(id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            if (!User.Identity.IsAuthenticated || !IsEventBeheerder(evenement))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(evenement);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Evenement evenementProxy = db.Evenements.Find(id);
            //Create non proxy copy
            Evenement evenement = new Evenement
            {
                evenement_id = id,
                naam = evenementProxy.naam,
                beschrijving = evenementProxy.beschrijving,
                begindatum = evenementProxy.begindatum,
                einddatum = evenementProxy.einddatum
            };

            if (db.Evenements.Find(evenement.evenement_id).EvenementBeheerders.Count() > 1)
            {
                TempData["wijziging"] = GenerateWijziging(evenement, 2);
                return RedirectToAction("Create", "Wijzigingen", null);
            }
            else
            {
                db.Evenements.Remove(evenementProxy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        //TODO wijziging maken voor beheerder toevoegen

        public ActionResult AddBeheerder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evenement evenement = db.Evenements.Find(id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            if (!User.Identity.IsAuthenticated || !IsEventBeheerder(evenement))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            EvenementBeheerder evenementBeheerder = new EvenementBeheerder
            {
                evenement = evenement.evenement_id
            };
            IEnumerable<Beheerder> beheerders = db.Beheerders.ToList().Where(b => !evenement.EvenementBeheerders.Select(e => e.beheerder).Contains(b.beheerder_id));
            ViewBag.beheerder = new SelectList(beheerders, "beheerder_id", "gebruikersnaam", "-1");

            return View(evenementBeheerder);
        }

        [HttpPost]
        public ActionResult AddBeheerder([Bind(Include = "evenement_beheerder_id, evenement, beheerder")] EvenementBeheerder evenementBeheerder)
        {
            if (ModelState.IsValid)
            {
                db.EvenementBeheerders.Add(evenementBeheerder);
                db.SaveChanges();
                return RedirectToAction("Dashboard", new { id = evenementBeheerder.evenement} );
            }

            return View(evenementBeheerder);
        }



        private bool IsEventBeheerder(Evenement evenement)
        {
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            return evenement.EvenementBeheerders.ToList().Select(b => b.beheerder).Contains(userId);
        }

        private Wijziging GenerateWijziging(Evenement evenement, int type)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(User.Identity.GetUserId());
                int beheerder = db.EvenementBeheerders.Where(e => e.evenement == evenement.evenement_id).First(b => b.beheerder == userId).evenement_beheerder_id;
                Wijziging change = new Wijziging
                {
                    beheerder = beheerder,
                    type = type,
                    jsonClassType = evenement.GetType().ToString(),
                    jsonData = new JavaScriptSerializer().Serialize(evenement)
                };
                return change;
            }
            else
            {
                throw (new Exception("No logon found"));
            }
        }
    }
}