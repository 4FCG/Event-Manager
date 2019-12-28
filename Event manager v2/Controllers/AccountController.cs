using Event_manager_v2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Event_manager_v2.Controllers
{
    public class AccountController : Controller
    {
        private DataModelContext db = new DataModelContext();
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(User.Identity.GetUserId());
                Beheerder currentUser = db.Beheerders.First(b => b.beheerder_id == userId);
                return View(currentUser);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Login(LoginForm form)
        {
            Beheerder user = db.Beheerders.FirstOrDefault(b => b.gebruikersnaam == form.gebruikersnaam);
            if ( user != null)
            {
                SignIn(user.beheerder_id.ToString(), user.gebruikersnaam);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.NotValidUser = "This user does not exist.";
            }
            return View();
        }

        public ActionResult Edit()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(User.Identity.GetUserId());
                Beheerder currentUser = db.Beheerders.First(b => b.beheerder_id == userId);
                return View(currentUser);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "beheerder_id,voornaam,achternaam,gebruikersnaam,wachtwoord")] Beheerder beheerder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beheerder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beheerder);
        }

        private void SignIn (string id, string gebruikersnaam, bool isPersistent = false)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
            claims.Add(new Claim(ClaimTypes.Name, gebruikersnaam));

            ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() 
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            }, identity);
        }

        private void SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}