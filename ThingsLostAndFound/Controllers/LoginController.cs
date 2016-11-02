using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class LoginController : Controller
    {
        private TLAFEntities db = new TLAFEntities();
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
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login details are wrong.");
                }
                return View(user);
        }

        private bool IsValid(string UserName, string UserPass)
        {
            bool IsValid = false;
            
            var userList = from p in db.InfoUsers select p;
            foreach (var p in userList)
            {
                if ((p.UserName == UserName) && (p.UserPass == UserPass))
                {
                    IsValid = true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("");
                }
            }
            return IsValid;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}