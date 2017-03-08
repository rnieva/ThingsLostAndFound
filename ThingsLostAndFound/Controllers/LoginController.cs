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
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class LoginController : Controller
    {
        private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor

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
                    //FormsAuthentication.SetAuthCookie(user.UserName, false);    // this action authenticate to user, set to user authenticated at HttpContext.Current.User
                    string infoUserTicket = GetInfoUserTicket(user.UserName);
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

        private bool IsValid(string UserName, string UserPass)
        {
            bool IsValid = false;
            //var userData = db.InfoUsers.Where(a => a.UserName.Equals(UserName)).FirstOrDefault(); // to get Salt User
            var userData = _IDBServices.GetInfoUserByNameContact(UserName);
            string passEncrypt = Crypto.Hash(UserPass, userData.UserSalt);
            if ((userData != null) && (userData.UserPass == passEncrypt)) 
            {
                IsValid = true;
            }

            //var user = db.InfoUsers.Where(a => a.UserName.Equals(UserName) && a.UserPass.Equals(passEncrypt)).FirstOrDefault();
            //if ((user != null) && (user.UserName == UserName)) //double check with username
            //{
            //    IsValid = true;
            //}

            return IsValid;
        }

        private string GetInfoUserTicket(string UserName)
        {
            string infoUserTicket = "0";
            string rol = "";
            string id = "";
            string newMessage = "";
            InfoUser user = new InfoUser();
            Message messageNew = new Message();
            //if ((user = db.InfoUsers.Where(a => a.UserName.Equals(UserName)).FirstOrDefault()) != null)
            if ((user = _IDBServices.GetInfoUserByNameContact(UserName)) != null)
            {
                id = user.Id.ToString();
                rol = user.Rol.ToString();
                // search in Message table if there is new messages for this user
                // if ((messageNew = db.Messages.Where((a => a.UserIdDest == user.Id && a.NewMessage == true)).FirstOrDefault()) != null)
                //if ((messageNew =  != null)
                //{
                //    newMessage = "True"; // temp
                //}
                //else
                //{
                //    newMessage = "False"; // temp
                //}
                newMessage = _IDBServices.CheckNewMessage(user.Id);

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