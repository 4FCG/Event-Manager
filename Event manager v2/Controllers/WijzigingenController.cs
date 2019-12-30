using Event_manager_v2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
    }
}