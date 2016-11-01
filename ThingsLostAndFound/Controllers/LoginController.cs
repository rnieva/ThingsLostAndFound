using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ThingsLostAndFound.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.InfoUser user)
        {
                if (IsValid(user.UserName, user.UserPass))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login details are wrong.");
                }
                return View(user);
        }

        private bool IsValid(string email, string password)
        {
            //var crypto = new SimpleCrypto.PBKDF2();
            bool IsValid = false;

            //using (var db = new LoginInMVC4WithEF.Models.UserEntities2())
            //{
            //    var user = db.Registrations.FirstOrDefault(u => u.Email == email);
            //    if (user != null)
            //    {
            //        if (user.Password == crypto.Compute(password, user.PasswordSalt))
            //        {
            //            IsValid = true;
            //        }
            //    }
            //}
            return IsValid;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}