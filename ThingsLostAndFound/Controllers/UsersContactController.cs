using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class UsersContactController : Controller    // This controller do the communication with users
    {
        private TLAFEntities db = new TLAFEntities();
        // GET: UsersContact
        public ActionResult ContactUserFoundObject(int id, string title, string userName, string securityQuestion) //When a user has lost a object and see it in the Found Objects List o Found Object Map, the user uses this method for contact with user that found the object
        {
            //This info cames from listFO, It show in the form
            ViewBag.idObject = id;
            ViewBag.titleObject = title;
            ViewBag.userName = userName;
            ViewBag.securityQuestion = securityQuestion;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string infoUserIdRolNewM = ticket.UserData.ToString();
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                InfoUser userRequest = new InfoUser();
                userRequest = db.InfoUsers.Find(userId);
                ViewBag.userIdRequest = userId;
                ViewBag.userNameRequest = userRequest.UserName; // This user want to do the request
                //If the user is register, it show this view, without email
                return View("ContactUserRegisterFoundObject");
            }
            //If the user isn´t registered, it show this view with fileds in the form, we need the contact email
            return View();    //show a form to do a request to userFinder
        }

        [HttpPost]
        public ActionResult SendRequestUser(int id, string textMessage, string emailUserLostObject)  //send an email from user lost object (user that do request (Not registered in the app)) to user that found the object
        {
            //with id from object, search the user that found the object and send him a email
            var foundObject = db.FoundObjects.Find(id);
            string SecurityQuestion = foundObject.SecurityQuestion;
            int userIdReport = foundObject.UserIdreported;
            string nameUserFounfObject = foundObject.InfoUser.UserName;
            string emailUserFoundObject = foundObject.InfoUser.Email;
            string buildBodyEmail = BuildBodyEmail(textMessage, emailUserLostObject);
            if (sendEmailToUserThatFoundTheObject(buildBodyEmail, emailUserLostObject, id) == true)
            {
                ViewBag.result = "Request sent successfull";
                //Store in DB data about contact between users, depends if the user is register or not
                //the user isn´t registered
                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = textMessage;
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = null; // null because the user that do the request is not registered
                msg.UserIdDest = userIdReport; //user that found the object
                msg.subject = "Found Object";
                msg.FoundObjectId = id; // id object
                msg.LostObjectId = null;
                msg.emailAddressUserDontRegis = emailUserLostObject; // user not registered
                db.Messages.Add(msg);
                db.SaveChanges();
            }
            else
            {
                ViewBag.result = "Request don´t sent";

            }
            return View(); // show the view with the result, successfull or not successfull if the email was sended
        }

        public string BuildBodyEmail(string textMessage, string emailUserLostObject)
        {
            string buildBodyEmail = "";
            buildBodyEmail = "Request from:<b> " + emailUserLostObject + "</b><br>"
                       + "<b>Message:</b><br> " + textMessage + "<br>"
                       + "---------------------------------------------------------<br>"
                       + "Thanks for your help, please answer email<b> " + emailUserLostObject + "</b><br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return buildBodyEmail;
        }

        protected bool sendEmailToUserThatFoundTheObject(string buildBodyEmail, string emailUserLostObject, int id)
        {
            string emailrecipient = System.Configuration.ConfigurationManager.AppSettings["testRecipientEmailCredentialvalue"];  //email recipient, //here go emailUserLostObject if the user isn´t registerd or emailUserRequest if the user is registerd
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(emailrecipient));
            email.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailCredentialvalue"], "ThingsLostAndFound");
            email.Subject = "Object ID: " + id.ToString() + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            email.Body = buildBodyEmail;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.live.com";
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["emailCredentialvalue"], System.Configuration.ConfigurationManager.AppSettings["passEmailCredentialvalue"]); // email and pass user
            try
            {
                smtp.Send(email);
                email.Dispose();
                System.Diagnostics.Debug.WriteLine("Email sent");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error to sent email: " + ex.Message);
                return false;
            }

        }

        [HttpPost]
        public ActionResult SendRequestRegisteredUser(int id, int usetIdRequest, string textMessage)
        {
            //with id from object, search the user that found the object and send him a email
            var foundObject = db.FoundObjects.Find(id);
            string SecurityQuestion = foundObject.SecurityQuestion;
            int userIdReport = foundObject.UserIdreported;
            string nameUserFounfObject = foundObject.InfoUser.UserName;
            string emailUserFoundObject = foundObject.InfoUser.Email;
            //with userIdReport from user that do the request, search the email of user
            var userRequest = db.InfoUsers.Find(usetIdRequest);
            string emailUserRequest = userRequest.Email;
            string buildBodyEmail = BuildBodyEmail(textMessage, emailUserRequest);
            if (sendEmailToUserThatFoundTheObject(buildBodyEmail, emailUserRequest, id) == true)
            {
                ViewBag.result = "Request sent successfull";
                //Store in DB data about contact between users, depends if the user is register or not
                //the user is registered
                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = textMessage;
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = usetIdRequest; // user that found the object
                msg.UserIdDest = userIdReport; //user that found the object
                msg.subject = "Found Object";
                msg.FoundObjectId = id; // id object
                msg.LostObjectId = null;
                msg.emailAddressUserDontRegis = null; // user not registered
                db.Messages.Add(msg);
                db.SaveChanges();
            }
            else
            {
                ViewBag.result = "Request don´t sent"; //TODO: come back to Messages view

            }
            return View("SendRequestUser"); //TODO: come back to Messages view
        }


        public ActionResult SendMessage(int id, int userSendMsg, int userDestMsg, string subject)
        { // this Actuon show a view for the user write the message and submit
            ViewBag.idObject = id;
            ViewBag.userSendMsg = userSendMsg;
            ViewBag.userDestMsg = userDestMsg;
            ViewBag.subject = subject;
            if (id != 0) //if id=0 is a message for support
            {
                if (subject == "Found Object")
                {
                    var foundObject = db.FoundObjects.Find(id);
                    ViewBag.titleObject = foundObject.Title;
                    ViewBag.userNameSend = foundObject.InfoUser.UserName;
                    var infouser = db.InfoUsers.Find(userDestMsg);
                    ViewBag.userNameDest = infouser.UserName;
                }
                else
                {
                    // the subject is Lost Object
                    var lostObject = db.LostObjects.Find(id);
                    ViewBag.titleObject = lostObject.Title;
                    ViewBag.userNameSend = lostObject.InfoUser.UserName;
                    var infouser = db.InfoUsers.Find(userDestMsg);
                    ViewBag.userNameDest = infouser.UserName;
                }
            }
            else
            {
                ViewBag.titleObject = "Support";
                var infouser = db.InfoUsers.Find(userSendMsg);
                ViewBag.userNameSend = infouser.UserName;
                infouser = db.InfoUsers.Find(userDestMsg);
                ViewBag.userNameDest = infouser.UserName;
            }
            return View();
        }

        public string BuildBodyEmail2(string textMessage, string nameUserSend, string emailUserLostObject)
        {
            string buildBodyEmail = "";
            buildBodyEmail = "Request from:<b> " + nameUserSend + " email: " + emailUserLostObject + "</b><br>"
                       + "<b>Message:</b><br> " + textMessage + "<br>"
                       + "---------------------------------------------------------<br>"
                       + "Thanks for your help, please answer email<b> " + emailUserLostObject + "</b><br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return buildBodyEmail;
        }


        [HttpPost]
        public ActionResult SendMessage2(int id, int userSendMsg, int userDestMsg, string subject, string textMessage)
        {   // this Action send the message
            //with id from object, search the user that found the object and send him a email
            string title = "";
            if (id != 0)
            {
                switch (subject)
                {
                    case "Found Object":
                        var foundObject = db.FoundObjects.Find(id);
                        title = foundObject.Title;
                        break;
                    case "Lost Object":
                        var lostObject = db.LostObjects.Find(id);
                        title = lostObject.Title;
                        break;
                }
            } //TODO: use title or remove

            var infouser = db.InfoUsers.Find(userSendMsg);
            string emailUserSend = infouser.Email;
            string nameUserSend = infouser.UserName;

            infouser = db.InfoUsers.Find(userDestMsg);
            string emailUserDest = infouser.Email;
            string nameUserDest = infouser.UserName;


            string buildBodyEmail = BuildBodyEmail2(textMessage, nameUserSend, emailUserSend);
            if (sendEmailToUserThatFoundTheObject(buildBodyEmail, emailUserSend, id) == true)
            {
                ViewBag.result = "Request sent successfull";
                //Store in DB data about contact between users

                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = textMessage;
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = userSendMsg;
                msg.UserIdDest = userDestMsg;
                msg.subject = subject;
                switch (subject)
                {
                    case "Support":
                        msg.FoundObjectId = null; // id object
                        msg.LostObjectId = null;
                        break;
                    case "Found Object":
                        msg.FoundObjectId = id; // id object
                        msg.LostObjectId = null;
                        break;
                    case "Lost Object":
                        msg.FoundObjectId = null; // id object
                        msg.LostObjectId = id;
                        break;
                }
                msg.emailAddressUserDontRegis = null; // user not registered
                db.Messages.Add(msg);
                db.SaveChanges();
            }
            else
            {
                ViewBag.result = "Request don´t sent"; //TODO: come back to Messages view

            }
            return View("SendRequestUser"); //TODO: come back to Messages view
        }
    }
}