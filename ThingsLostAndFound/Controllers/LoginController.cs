using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Security;

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
            string passEncrypt = Crypto.Hash(user.UserPass); //TODO add Salted
            if (IsValid(user.UserName, passEncrypt))
                {
                    //FormsAuthentication.SetAuthCookie(user.UserName, false);    // this action authenticate to user, set to user authenticated at HttpContext.Current.User
                    string infoUserTicket = GetInfoUserTicket(user.UserName, passEncrypt);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        user.UserName,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                        infoUserTicket,   // Add User Id and Role, later we can retrive these data instead of read from DB
                        FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    Response.Redirect(FormsAuthentication.GetRedirectUrl(user.UserName, false));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login details are wrong.");
                }
                return View(user);
        }

        private bool IsValid(string UserName, string passEncrypt)
        {
            bool IsValid = false;
            var user = db.InfoUsers.Where(a => a.UserName.Equals(UserName) && a.UserPass.Equals(passEncrypt)).FirstOrDefault();
            if ((user != null) && (user.UserName == UserName)) //double check with username
            {
                IsValid = true;
            }

            //var userList = from p in db.InfoUsers select p;
            //foreach (var p in userList)
            //{
            //    if ((p.UserName == UserName) && (p.UserPass == passEncrypt))
            //    {
            //        IsValid = true;
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("");
            //    }
            //}

            return IsValid;
        }

        private string GetInfoUserTicket(string UserName, string passEncrypt)
        {
            string infoUserTicket = "0";
            string rol = "";
            string id = "";
            string newMessage = "";
            InfoUser user = new InfoUser();
            Message messageNew = new Message();
            if ((user = db.InfoUsers.Where(a => a.UserName.Equals(UserName) && a.UserPass.Equals(passEncrypt)).FirstOrDefault()) != null)
            {
                id = user.Id.ToString();
                rol = user.Rol.ToString();
                // search in Message table if there is new messages for this user
                if ((messageNew = db.Messages.Where((a => a.UserIdDest == user.Id && a.NewMessage == true)).FirstOrDefault()) != null)
                {
                    newMessage = "True"; // temp
                }
                else
                {
                    newMessage = "False"; // temp
                }
                infoUserTicket = id + "|" + rol + "||" + newMessage;
            }
            return infoUserTicket;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}