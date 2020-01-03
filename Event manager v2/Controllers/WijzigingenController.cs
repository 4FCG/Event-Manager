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
    public class WijzigingenController : Controller
    {
        private DataModelContext db = new DataModelContext();

        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(User.Identity.GetUserId());
                List<int> allUserLinks = db.EvenementBeheerders.Where(m => m.beheerder == userId).Select(m => m.evenement_beheerder_id).ToList();
                //convert to int?
                return View(db.Wijzigings.Where(m => allUserLinks.Contains(m.beheerder)).ToList());
            }
            return RedirectToAction("Login", "Account");
        }
        //TODO toon de naam van de beheerder van een wijziging
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wijziging wijziging = db.Wijzigings.Find(id);
            if (wijziging == null)
            {
                return HttpNotFound();
            }
            if (!db.IsAuthorized(wijziging.beheerder, User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(wijziging);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteComfirmed(int id)
        {
            Wijziging wijziging = db.Wijzigings.Find(id);
            if (!db.IsAuthorized(wijziging.beheerder, User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            db.Wijzigings.Remove(wijziging);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EvenementWijzigingen(int? evenement_id)
        {
            if (evenement_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            //TODO check if user is beheerder for evenement
            Evenement evenement = db.Evenements.Find(evenement_id);
            if (evenement == null)
            {
                return HttpNotFound();
            }
            ViewBag.EvenementNaam = evenement.naam;
            ViewBag.EvenementId = evenement.evenement_id;
            ViewBag.UserId = User.Identity.GetUserId();

            IEnumerable<Wijziging> wijzigingen = db.Wijzigings.ToList().Where(w => evenement.EvenementBeheerders.Select(m => m.evenement_beheerder_id).Contains(w.beheerder));
            return View(wijzigingen);
        }

        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wijziging wijziging = db.Wijzigings.Find(id);
            if (wijziging == null)
            {
                return HttpNotFound();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            //TODO check if user is not the maker of the wijziging, check if user is part of evenement
            int eventId = db.Evenements.Find(db.EvenementBeheerders.Find(wijziging.beheerder).evenement).evenement_id;
            ExecuteWijziging(wijziging);
            return RedirectToAction("Dashboard", "Evenementen", new { id = eventId });
        }

        public ActionResult Create()
        {
            Wijziging wijziging = TempData["wijziging"] as Wijziging;
            if (wijziging == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(wijziging);
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "wijziging_id,beheerder,type,naam,beschrijving,jsonData,jsonClassType")] Wijziging wijziging)
        {
            if (ModelState.IsValid)
            {
                db.Wijzigings.Add(wijziging);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Evenementen", new { id = db.Evenements.Find(db.EvenementBeheerders.Find(wijziging.beheerder).evenement).evenement_id });
            }

            return View(wijziging);
        }

        //Reads the wijziging and executes the stored database change
        private void ExecuteWijziging(Wijziging wijziging)
        {
            object Wijzigingobject = new JavaScriptSerializer().Deserialize(wijziging.jsonData, Type.GetType(wijziging.jsonClassType));
            string type = wijziging.jsonClassType.Substring(24);
            //Insert
            if (wijziging.WijzigingsType.type_id == 1)
            {
                if (type == "Evenement")
                {
                    db.Evenements.Add(Wijzigingobject as Evenement);
                }
                else if (type == "Activiteit")
                {
                    db.Activiteits.Add(Wijzigingobject as Activiteit);
                }
            }
            //Delete
            else if (wijziging.WijzigingsType.type_id == 2)
            {
                if (type == "Evenement")
                {
                    db.Evenements.Remove(db.Evenements.Find((Wijzigingobject as Evenement).evenement_id));
                }
                else if (type == "Activiteit")
                {
                    db.Activiteits.Remove(db.Activiteits.Find((Wijzigingobject as Activiteit).activiteit_id));
                }
            }
            //Update
            //TODO object can be removed before Update wijziging is used creating a concurrency exeption
            else if (wijziging.WijzigingsType.type_id == 3)
            {
                if (type == "Evenement")
                {
                    db.Entry(Wijzigingobject as Evenement).State = EntityState.Modified;
                }
                else if (type == "Activiteit")
                {
                    db.Entry(Wijzigingobject as Activiteit).State = EntityState.Modified;
                }
            }
            db.Wijzigings.Remove(wijziging);
            db.SaveChanges();
        }
    }
}