using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Event_manager_v2.Models;

namespace Event_manager_v2.Controllers
{
    public class DeelnemersController : Controller
    {
        private DataModelContext db = new DataModelContext();

        // GET: Deelnemers
        public ActionResult Index()
        {
            var deelnemers = db.Deelnemers.Include(d => d.Evenement1);
            return View(deelnemers.ToList());
        }

        // GET: Deelnemers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deelnemer deelnemer = db.Deelnemers.Find(id);
            if (deelnemer == null)
            {
                return HttpNotFound();
            }
            return View(deelnemer);
        }

        // GET: Deelnemers/SchrijfIn
        [HttpGet]
        public ActionResult SchrijfIn(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Deelnemer deelnemer = new Deelnemer { evenement = (int)id };

                return View(deelnemer);

            }
        }

        // POST: Deelnemers/SchrijfIn
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult SchrijfIn([Bind(Include = "deelnemer_id,voornaam,achternaam,email,evenement,goedgekeurd")] Deelnemer deelnemer)
        {
            if (ModelState.IsValid)
            {
                db.Deelnemers.Add(deelnemer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deelnemer);
        }

        // GET: Deelnemers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deelnemer deelnemer = db.Deelnemers.Find(id);
            if (deelnemer == null)
            {
                return HttpNotFound();
            }
            
            return View(deelnemer);
        }

        // POST: Deelnemers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "deelnemer_id,voornaam,achternaam,email,evenement,goedgekeurd")] Deelnemer deelnemer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deelnemer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deelnemer);
        }

        // GET: Deelnemers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deelnemer deelnemer = db.Deelnemers.Find(id);
            if (deelnemer == null)
            {
                return HttpNotFound();
            }
            return View(deelnemer);
        }

        // POST: Deelnemers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deelnemer deelnemer = db.Deelnemers.Find(id);
            db.Deelnemers.Remove(deelnemer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET Deelnemers/Goedkeuren/id
        [HttpGet]
        public ActionResult Goedkeuren(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deelnemer deelnemer = db.Deelnemers.Find(id);
            if (deelnemer == null)
            {
                return HttpNotFound();
            }
            ViewBag.evenement = new SelectList(db.Evenements, "evenement_id", "naam", deelnemer.evenement);
            return View(deelnemer);

        }

        //POST Deelnemers/Goedkeuren/id
        [HttpPost]
        public ActionResult Goedkeuren([Bind(Include = "deelnemer_id, voornaam, achternaam, email, evenement, goedgekeurd")] Deelnemer deelnemer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deelnemer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deelnemer);
        } 

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
