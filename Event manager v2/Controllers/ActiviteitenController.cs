using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Event_manager_v2.Models;
using Microsoft.AspNet.Identity;

namespace Event_manager_v2.Controllers
{
    public class ActiviteitenController : Controller
    {
        private DataModelContext db = new DataModelContext();

        // GET: Activiteiten
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", null);
        }

        // GET: Activiteiten/Create
        public ActionResult Create(int? evenement_id)
        {
            if (evenement_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            EvenementBeheerder evenementBeheerder = db.Evenements.Find(evenement_id).EvenementBeheerders.FirstOrDefault(b => b.beheerder == userId);
            if (evenementBeheerder == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Activiteit activiteit = new Activiteit { 
                evenement = (int)evenement_id,
                evenement_beheerder = evenementBeheerder.evenement_beheerder_id
            };
            return View(activiteit);
        }

        // POST: Activiteiten/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "activiteit_id,naam,beschrijving,evenement,begintijd,eindtijd,evenement_beheerder")] Activiteit activiteit)
        {
            if (ModelState.IsValid)
            {
                
                if (db.Evenements.Find(activiteit.evenement).EvenementBeheerders.Count() > 1)
                {
                    TempData["wijziging"] = GenerateWijziging(activiteit, 1);
                    return RedirectToAction("Create", "Wijzigingen", null);
                }
                else
                {
                    db.Activiteits.Add(activiteit);
                    db.SaveChanges();
                }
                return RedirectToAction("Dashboard", "Evenementen", new { id = activiteit.evenement });
            }

            return View(activiteit);
        }

        // GET: Activiteiten/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Activiteit activiteit = db.Activiteits.Find(id);
            if (activiteit == null)
            {
                return HttpNotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            EvenementBeheerder evenementBeheerder = db.Evenements.Find(activiteit.evenement).EvenementBeheerders.FirstOrDefault(b => b.beheerder == userId);
            if (evenementBeheerder == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(activiteit);
        }

        // POST: Activiteiten/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "activiteit_id,naam,beschrijving,evenement,begintijd,eindtijd,evenement_beheerder")] Activiteit activiteit)
        {
            if (ModelState.IsValid)
            {
                if (db.Evenements.Find(activiteit.evenement).EvenementBeheerders.Count() > 1)
                {
                    TempData["wijziging"] = GenerateWijziging(activiteit, 3);
                    return RedirectToAction("Create", "Wijzigingen", null);
                }
                else
                {
                    db.Entry(activiteit).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", "Evenementen", new { id = activiteit.evenement });
                }
            }

            return View(activiteit);
        }
        //TODO Check if user is allowed to make changes to the activiteit.
        // GET: Activiteiten/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            Activiteit activiteit = db.Activiteits.Find(id);
            if (activiteit == null)
            {
                return HttpNotFound();
            }
            
            return View(activiteit);
        }

        // POST: Activiteiten/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Activiteit activiteitProxy = db.Activiteits.Find(id);
            //Create non proxy copy
            Activiteit activiteit = new Activiteit 
            {
                activiteit_id = id,
                naam = activiteitProxy.naam,
                beschrijving = activiteitProxy.beschrijving,
                evenement = activiteitProxy.evenement,
                evenement_beheerder = activiteitProxy.evenement_beheerder,
                begintijd = activiteitProxy.begintijd,
                eindtijd = activiteitProxy.eindtijd
            };

            if (db.Evenements.Find(activiteit.evenement).EvenementBeheerders.Count() > 1)
            {
                TempData["wijziging"] = GenerateWijziging(activiteit, 2);
                return RedirectToAction("Create", "Wijzigingen", null);
            }
            else
            {
                db.Activiteits.Remove(activiteit);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Evenementen", new { id = activiteit.evenement });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Wijziging GenerateWijziging (Activiteit activiteit, int type)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(User.Identity.GetUserId());
                int beheerder = db.EvenementBeheerders.Where(e => e.evenement == activiteit.evenement).First(b => b.beheerder == userId).evenement_beheerder_id;
                Wijziging change = new Wijziging
                {
                    beheerder = beheerder,
                    type = type,
                    jsonClassType = activiteit.GetType().ToString(),
                    jsonData = new JavaScriptSerializer().Serialize(activiteit)
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
