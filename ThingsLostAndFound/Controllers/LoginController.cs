using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
                    FormsAuthentication.SetAuthCookie(user.UserName, false);    // this action authenticate to user, set to user authenticated at HttpContext.Current.User
                    string rol = GetRol(user.UserName, user.UserPass);
                    //TODO: add the user to role to avoid check the role in RoleAuthorizationAttribute class
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

           // improvement, use this to check the credentials
            var user = db.InfoUsers.Where(a => a.UserName.Equals(UserName) && a.UserPass.Equals(UserPass)).FirstOrDefault();

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

        private string GetRol(string UserName, string UserPass)
        {
            string rol = "0";
            InfoUser user = new InfoUser();
            if ((user = db.InfoUsers.Where(a => a.UserName.Equals(UserName) && a.UserPass.Equals(UserPass)).FirstOrDefault()) != null)
            {
                rol = user.Rol.ToString();
            }
            return rol;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}