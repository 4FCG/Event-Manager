using Event_manager_v2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "evenement_id,naam,beschrijving,begindatum,einddatum")] Evenement evenement)
        {
            if(ModelState.IsValid)
            {
                var change = new Wijziging();
                change.beheerder = 1;
                change.beschrijving = "Json test";
                change.type = 1;
                change.naam = "Json";

                change.jsonClassType = evenement.GetType().ToString();
                change.jsonData = new JavaScriptSerializer().Serialize(evenement);

                db.Wijzigings.Add(change);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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

            EvenementDashboardViewModel viewmodel = new EvenementDashboardViewModel
            {
                Evenement = evenement,
                Activiteiten = evenement.Activiteits,
                Beheerders = beheerders
            };

            return View(viewmodel);
        }
    }
}