using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class UsersContactController : Controller    // This controller do the communication with users
    {
        //private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor

        //Communication for Found Object, from users LO to users FO
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
                //InfoUser userRequest = new InfoUser();
                //userRequest = db.InfoUsers.Find(userId);
                InfoUser userRequest =_IDBServices.GetInfoUser(userId);
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
            //var foundObject = db.FoundObjects.Find(id);
            var foundObject = _IDBServices.GetDetailsFO(id);
            string SecurityQuestion = foundObject.SecurityQuestion;
            int userIdReport = foundObject.UserIdreported;
            string nameUserFounfObject = foundObject.InfoUser.UserName;
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
            msg.emailAddressUserDontRegis = emailUserLostObject; // only for user not registered
            //db.Messages.Add(msg);
            _IDBServices.AddMessage(msg);
            //db.SaveChanges();
            _IDBServices.SaveChanges();
            //send email
            //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
            Setting settings = _IDBServices.GetSettings();
            if (settings.SendMsgFoUserNR == true)
            {
                string emailSubject = "Object ID: " + id.ToString() + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                string emailBody = sendEmail.BuildBodyEmailUserLOToUserFO(textMessage, emailUserLostObject);
                string emailRecipient = foundObject.InfoUser.Email;
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    ViewBag.result = "Request sent successfull";
                }
                else
                {
                    ViewBag.result = "Request don´t sent by email";
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("send email FO user Not Reg not activated");
                ViewBag.result = "send email FO user not Reg not activated";
            }
            return View(); // show the view with the result, successfull or not successfull if the email was sended
        }

        [HttpPost]
        public ActionResult SendRequestRegisteredUser(int id, int usetIdRequest, string textMessage)
        {
            //with id from object, search the user that found the object and send him a email
            //var foundObject = db.FoundObjects.Find(id);
            var foundObject = _IDBServices.GetDetailsFO(id);
            string SecurityQuestion = foundObject.SecurityQuestion;
            int userIdReport = foundObject.UserIdreported;
            string nameUserFounfObject = foundObject.InfoUser.UserName;
            string emailRecipient = foundObject.InfoUser.Email;
            //with userIdReport from user that do the request, search the email of user
            //var userRequest = db.InfoUsers.Find(usetIdRequest);
            var userRequest = _IDBServices.GetInfoUser(usetIdRequest);
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
            msg.emailAddressUserDontRegis = null; // only for user not registered
            //db.Messages.Add(msg);
            _IDBServices.AddMessage(msg);
            //db.SaveChanges();
            _IDBServices.SaveChanges();
            //send email
            //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
            Setting settings = _IDBServices.GetSettings();
            if (settings.SendMsgFoUserReg == true)
            {
                string emailUserLostObject = userRequest.Email;
                string emailSubject = "Object ID: " + id.ToString() + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                string emailBody = sendEmail.BuildBodyEmailUserLOToUserFO(textMessage, emailUserLostObject);
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    ViewBag.result = "Request sent successfull";
                }
                else
                {
                    ViewBag.result = "Request don´t sent by email"; //TODO: come back to Messages view
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("send email FO user Reg not activated");
                ViewBag.result = "send email FO user Reg not activated";
            }
            return View("SendRequestUser"); //TODO: come back to Messages view
        }

        //Communication for Lost Object, from users FO to users LO
        
        public ActionResult ContactUserLostObject(int id, string title, string userName) //When a user has found a object and see it in the lost Objects List o lost Object Map, the user uses this method for contact with user that lost the object
        {
            //This info cames from listLO, It show in the form "Hi pal, I have your object"
            ViewBag.idObject = id;
            ViewBag.titleObject = title;
            ViewBag.userName = userName;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string infoUserIdRolNewM = ticket.UserData.ToString();
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                //InfoUser userRequest = new InfoUser();
                //userRequest = db.InfoUsers.Find(userId);
                InfoUser userRequest = _IDBServices.GetInfoUser(userId);
                ViewBag.userIdRequest = userId;
                ViewBag.userNameRequest = userRequest.UserName; // This user want to do the request
                //If the user is register, it show this view, without email
                return View("ContactUserRegisterLostObject");
            }
            //If the user isn´t registered, it show this view with fileds in the form, we need the contact email
            return View();    //show a form to do a request to userFinder
        }

        [HttpPost]
        public ActionResult SendRequestUser2(int id, string textMessage, string emailUserFoundObject)  //send an email from user found object because he saw it (user not registered) in the lost object list to user that lost the object
        {
            //with id from object, search the user that found the object and send him a email
            //var lostObject = db.LostObjects.Find(id);
            var lostObject = _IDBServices.GetDetailsLO(id);
            int userIdReport = lostObject.UserIdreported;
            string nameUserLostObject = lostObject.InfoUser.UserName;
            //Store in DB data about contact between users, depends if the user is register or not
            //the user isn´t registered
            Message msg = new Message();
            msg.NewMessage = true;
            msg.Message1 = textMessage;
            msg.dateMessage = DateTime.Now;
            msg.UserIdSent = null; // null because the user that do the request is not registered
            msg.UserIdDest = userIdReport; //user that found the object
            msg.subject = "Lost Object";
            msg.FoundObjectId = null;
            msg.LostObjectId = id; // id object
            msg.emailAddressUserDontRegis = emailUserFoundObject; // only for user not registered
            //db.Messages.Add(msg);
            _IDBServices.AddMessage(msg);
            //db.SaveChanges();
            _IDBServices.SaveChanges();
            //send email
            //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
            Setting settings = _IDBServices.GetSettings();
            if (settings.SendMsgLoUserNR == true)
            {
                string emailRecipient = lostObject.InfoUser.Email;
                string emailSubject = "Object ID: " + id.ToString() + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                string emailBody = sendEmail.BuildBodyEmailUserFOToUserLO(textMessage, emailUserFoundObject);
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    ViewBag.result = "Request sent successfull";

                }
                else
                {
                    ViewBag.result = "Request don´t sent by email";
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("send email LO user not Reg not activated");
                ViewBag.result = "send email LO user not Reg not activated";
            }
            return View("SendRequestUser"); // show the view with the result, successfull or not successfull if the email was sended
        }

        [HttpPost]
        public ActionResult SendRequestRegisteredUser2(int id, int usetIdRequest, string textMessage)
        {   // from ContactUserRegisterLostObject
            //with id from object, search the user that found the object and send him a email
            //var lostObject = db.LostObjects.Find(id);
            var lostObject = _IDBServices.GetDetailsLO(id);
            int userIdReport = lostObject.UserIdreported;
            string nameUserLostObject = lostObject.InfoUser.UserName;
            string emailRecipient = lostObject.InfoUser.Email;
            //with userIdReport from user that do the request, search the address email 
            //var userRequest = db.InfoUsers.Find(usetIdRequest);
            var userRequest = _IDBServices.GetInfoUser(usetIdRequest);
            //Store in DB data about contact between users, depends if the user is register or not
            //the user is registered
            Message msg = new Message();
            msg.NewMessage = true;
            msg.Message1 = textMessage;
            msg.dateMessage = DateTime.Now;
            msg.UserIdSent = usetIdRequest; // user that found the object
            msg.UserIdDest = userIdReport; //user that found the object
            msg.subject = "Lost Object";
            msg.FoundObjectId = null;
            msg.LostObjectId = id; // id object
            msg.emailAddressUserDontRegis = null; // only for user not registered
            //db.Messages.Add(msg);
            _IDBServices.AddMessage(msg);
            //db.SaveChanges();
            _IDBServices.SaveChanges();
            //send email
            //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
            Setting settings = _IDBServices.GetSettings();
            if (settings.SendMsgLoUserReg == true)
            {
                string emailUserRequest = userRequest.Email;
                string emailSubject = "Object ID: " + id.ToString() + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                string emailBody = sendEmail.BuildBodyEmailUserFOToUserLO(textMessage, emailUserRequest);
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    ViewBag.result = "Request sent successfull";
                }
                else
                {
                    ViewBag.result = "Request don´t sent by email"; //TODO: come back to Messages view
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("send email LO user Reg not activated");
                ViewBag.result = "send email LO user Reg not activated";
            }
            return View("SendRequestUser"); //TODO: come back to Messages view
        }

        // Messages

        public ActionResult SendMessage(int id, int userSendMsg, int userDestMsg, string subject)
        { // this Action show a view for the user write the message and submit, its called from ShowMessages View
            ViewBag.idObject = id;
            ViewBag.userSendMsg = userSendMsg;
            ViewBag.userDestMsg = userDestMsg;
            ViewBag.subject = subject;
            if (id != 0) //if id=0 is a message for support
            {
                if (subject == "Found Object")
                {   // the subject is Found Object
                    //var foundObject = db.FoundObjects.Find(id);
                    var foundObject = _IDBServices.GetDetailsFO(id);
                    ViewBag.titleObject = foundObject.Title;
                    //var infoUserSendMsg = db.InfoUsers.Find(userSendMsg);
                    var infoUserSendMsg = _IDBServices.GetInfoUser(userSendMsg);
                    ViewBag.userNameSend = infoUserSendMsg.UserName;
                    var infouserDestMsg = _IDBServices.GetInfoUser(userDestMsg);
                    ViewBag.userNameDest = infouserDestMsg.UserName;
                }
                else
                {
                    // the subject is Lost Object
                    //var lostObject = db.LostObjects.Find(id);
                    var lostObject = _IDBServices.GetDetailsLO(id);
                    ViewBag.titleObject = lostObject.Title;
                    //var infoUserSendMsg = db.InfoUsers.Find(userSendMsg);
                    var infoUserSendMsg = _IDBServices.GetInfoUser(userSendMsg);
                    ViewBag.userNameSend = infoUserSendMsg.UserName;
                    //var infouserDestMsg = db.InfoUsers.Find(userDestMsg);
                    var infouserDestMsg = _IDBServices.GetInfoUser(userDestMsg);
                    ViewBag.userNameDest = infouserDestMsg.UserName;
                }
            }
            else
            {
                ViewBag.titleObject = "Support";
                //var infouser = db.InfoUsers.Find(userSendMsg);
                var infouser = _IDBServices.GetInfoUser(userSendMsg);
                ViewBag.userNameSend = infouser.UserName;
                //infouser = db.InfoUsers.Find(userDestMsg);
                infouser = _IDBServices.GetInfoUser(userDestMsg);
                ViewBag.userNameDest = infouser.UserName;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage2(int id, int userSendMsg, int userDestMsg, string subject, string textMessage)
        {   //this Action send the message
            //with id from object, search the user that found the object and send him a email
            string title = "";
            if (id != 0)
            {
                switch (subject)
                {
                    case "Found Object":
                        //var foundObject = db.FoundObjects.Find(id);
                        var foundObject = _IDBServices.GetDetailsFO(id);
                        title = foundObject.Title;
                        break;
                    case "Lost Object":
                        //var lostObject = db.LostObjects.Find(id);
                        var lostObject = _IDBServices.GetDetailsLO(id);
                        title = lostObject.Title;
                        break;
                }
            } 
            var infouser = _IDBServices.GetInfoUser(userSendMsg);
            string emailUserSend = infouser.Email;
            string nameUserSend = infouser.UserName;
            //infouser = db.InfoUsers.Find(userDestMsg);
            infouser = _IDBServices.GetInfoUser(userDestMsg);
            //Store in DB the message between the users
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
                    msg.FoundObjectId = null; 
                    msg.LostObjectId = id;  // id object
                    break;
            }
            msg.emailAddressUserDontRegis = null; // only for user not registered
            //db.Messages.Add(msg);
            _IDBServices.AddMessage(msg);
            //db.SaveChanges();
            _IDBServices.SaveChanges();
            //send email
            //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
            Setting settings = _IDBServices.GetSettings();
            if (settings.EmailMsgs == true)
            {
                string emailRecipient = infouser.Email; //emailUserDest
                string nameUserDest = infouser.UserName;
                string emailSubject = "Subject: " + subject + " Object: " + title + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                string emailBody = sendEmail.BuildBodyEmailMessage(textMessage, nameUserSend, emailUserSend);
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    ViewBag.result = "Request sent successfull";
                    
                }
                else
                {
                    ViewBag.result = "Request don´t sent"; //TODO: come back to Messages view

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("send email Msgs not activated");
                ViewBag.result = "send email Msgs not activated";
            }
            return View("SendRequestUser"); //TODO: come back to Messages view
        }
    }
}