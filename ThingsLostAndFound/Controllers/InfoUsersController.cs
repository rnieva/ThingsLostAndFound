using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Security;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class InfoUsersController : Controller
    {
        private TLAFEntities db = new TLAFEntities();

        // GET: InfoUsers
        [RoleAuthorization(Roles = "1")]
        public ActionResult Index()
        {
            return View(db.InfoUsers.ToList());
        }

        // GET: InfoUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string infoUserIdRolNewM = ticket.UserData.ToString();
                // It get user ID value from infoUserIdRolNewM
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                if (( id == userId) || (roll == 1))     // This way, only the user with hus id can see his details
                {
                    InfoUser infoUser = db.InfoUsers.Find(id);
                    return View(infoUser);
                }
                else
                {
                    return HttpNotFound();  //TODO: add view reject or error
                }
            }
            return HttpNotFound();
        }

        // GET: InfoUsers/AddUser   // Only the admin can use this method, the dofferent is that can set roles
        [RoleAuthorization(Roles = "1")]
        public ActionResult AddUser()
        {
            return View();
        }

        // POST: InfoUsers/AddUser
        [RoleAuthorization(Roles = "1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "Id,UserName,UserPass,Email,PhoneNumber,rol")] InfoUser infoUser)
        {
            if (ModelState.IsValid)
            {
                string passEncrypt = Crypto.Hash(infoUser.UserPass);
                infoUser.UserPass = passEncrypt;
                infoUser.Date = DateTime.Now;
                db.InfoUsers.Add(infoUser);
                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = "Welcome to TLAF";
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = 1; // id = 1, it`s the admin
                msg.UserIdDest = infoUser.Id;
                msg.subject = "Support";
                msg.FoundObjectId = null;
                msg.LostObjectId = null;
                msg.emailAddressUserDontRegis = null;
                db.Messages.Add(msg);
                db.SaveChanges();
                //send email
                Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
                if (settings.NewUser == true)
                {
                    string emailBody = sendEmail.NewUser(infoUser.UserName);
                    string emailSubject = infoUser.Id + "# New User ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                    string emailRecipient = infoUser.Email;
                    if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                    {
                        System.Diagnostics.Debug.WriteLine("email New User Sent");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("error send email");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(infoUser);
        }

        // GET: InfoUsers/Create
        public ActionResult Create()            // This is Register Menu Option
        {
            return View();
        }

        // POST: InfoUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,UserPass,Email,PhoneNumber")] InfoUser infoUser)
        {
            if (ModelState.IsValid)
            {
                //TODO: changed type of encrypt 
                string passEncrypt = Crypto.Hash(infoUser.UserPass);
                infoUser.UserPass = passEncrypt;
                infoUser.Rol = 3;  // ROL = 3 normal user
                infoUser.Date = DateTime.Now;
                db.InfoUsers.Add(infoUser);
                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = "Welcome to TLAF";
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = 1; // id = 1, it`s the admin
                msg.UserIdDest = infoUser.Id;
                msg.subject = "Support";
                msg.FoundObjectId = null;
                msg.LostObjectId = null;
                msg.emailAddressUserDontRegis = null;
                db.Messages.Add(msg);
                db.SaveChanges();
                //send email
                Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
                if (settings.NewUser == true)
                {
                    string emailBody = sendEmail.NewUser(infoUser.UserName);
                    string emailSubject = infoUser.Id + "# New User ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                    string emailRecipient = infoUser.Email;
                    if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                    {
                        System.Diagnostics.Debug.WriteLine("email New User Sent");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("error send email");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(infoUser);
        }

        // GET: InfoUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string infoUserIdRolNewM = ticket.UserData.ToString();
                // It get user ID value from infoUserIdRol
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                if ((id == userId) || (roll == 1))     // This way, only the user with hus id can see his details
                {
                    InfoUser infoUser = db.InfoUsers.Find(id);
                    return View(infoUser);
                }
                else
                {
                    return HttpNotFound();  //TODO: add view reject or error
                }
            }
            return HttpNotFound();
        }

        // POST: InfoUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,UserPass,Email,PhoneNumber,Rol,Date")] InfoUser infoUser, string oldPass, string newPass, string newPass2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(infoUser).State = EntityState.Modified;
                db.SaveChanges();
                //send email
                Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
                if (settings.EditUser == true)
                {
                    string emailBody = sendEmail.EditUser(infoUser.UserName);
                    string emailSubject = infoUser.Id + "# Edit User ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                    string emailRecipient = infoUser.Email;
                    if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                    {
                        System.Diagnostics.Debug.WriteLine("email New User Sent");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("error send email");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(infoUser);
        }

        
        public ActionResult ChangePass(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePass(int id, string oldPass, string newPass, string newPass2)
        {
            ViewBag.id = id;
            InfoUser infoUser = new InfoUser();
            infoUser = db.InfoUsers.Find(id);
            string oldPassEncrypt = Crypto.Hash(oldPass);
            string passNewEncrypt = Crypto.Hash(newPass);
            string passNew2Encrypt = Crypto.Hash(newPass2);
            if (oldPassEncrypt == infoUser.UserPass)
            {
                if (oldPassEncrypt == passNewEncrypt)
                {
                    ViewBag.msgPass = "The same password";
                    return View();
                }
                else
                {
                    if (passNewEncrypt == passNew2Encrypt)
                    {

                        infoUser.UserPass = passNewEncrypt;
                        db.Entry(infoUser).State = EntityState.Modified;
                        //db.SaveChanges();
                        string textMessage = "Password changed";
                        Message msg = new Message();
                        msg.NewMessage = true;
                        msg.Message1 = textMessage;
                        msg.dateMessage = DateTime.Now;
                        msg.UserIdSent = 1; // admin
                        msg.UserIdDest = id; //user that found the object
                        msg.subject = "Support";
                        msg.FoundObjectId = null; // id object
                        msg.LostObjectId = null;
                        msg.emailAddressUserDontRegis = null; // only for user not registered
                        db.Messages.Add(msg);
                        db.SaveChanges();
                        Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
                        if (settings.ChangePass == true)
                        {
                            string emailUserSend = "emailSupport@TLAF.com";
                            string emailRecipient = infoUser.Email;
                            string emailSubject = "Subject: Support - Pass changed " + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                            string emailBody = sendEmail.BuildBodyEmailMessage(textMessage, "Admin", emailUserSend);
                            if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                            {
                                return RedirectToAction("Logout", "Login");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("error send email");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("email to change pass no activated");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.msgPass = "It´s not the same password";
                        return View();
                    }

                }
            }
            else
            {
                ViewBag.msgPass = "Wrong Password";
                return View();
            }
        }

        public ActionResult sendNewPassByEmail(int? id)
        {
            Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
            if (settings.SendPass == true)
            {
                InfoUser infoUser = new InfoUser();
                infoUser = db.InfoUsers.Find(id);
                string newPass = Crypto.RandomString(6); //TODO: generate a random password
                string newPassEn = Crypto.Hash(newPass);
                infoUser.UserPass = newPassEn;
                db.Entry(infoUser).State = EntityState.Modified;
                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = "New Password sent by email, please change your new password";
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = 1; // admin
                msg.UserIdDest = id; //user that found the object
                msg.subject = "Support";
                msg.FoundObjectId = null; // id object
                msg.LostObjectId = null;
                msg.emailAddressUserDontRegis = null; // only for user not registered
                db.Messages.Add(msg);
                db.SaveChanges();
                string emailUserSend = "emailSupport@TLAF.com";
                string emailRecipient = infoUser.Email;
                string emailSubject = "Subject: Support - New Pass " + " Date ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                string textMessage = "New Password: " + newPass + " , please change your new password";
                string emailBody = sendEmail.BuildBodyEmailMessage(textMessage, "Admin", emailUserSend);
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    return RedirectToAction("Logout", "Login");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error sending email but new pass saved");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("email to send pass no activated");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: InfoUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string infoUserIdRolNewM = ticket.UserData.ToString();
                // It get user ID value from infoUserIdRol
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                if ((id == userId) || (roll == 1))     // This way, only the user with hus id can see his details
                {
                    InfoUser infoUser = db.InfoUsers.Find(id);
                    return View(infoUser);
                }
                else
                {
                    return HttpNotFound();  //TODO: add view reject or error
                }
            }
            return HttpNotFound();
        }

        // POST: InfoUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //TODO: delete every msg from to this user and FO and LO
            //var newMessagesUserList = db.Messages.Where(a => a.UserIdDest == id && a.UserIdSent == id).ToList();
            //foreach (var m in newMessagesUserList)
            //{
            //    db.Messages.Remove(m);
            //}
            //var FOlist = db.FoundObjects.Where(o => o.UserIdreported == id).ToList();
            //foreach (var fo in FOlist)
            //{
            //    db.FoundObjects.Remove(fo);
            //}
            //var LOlist = db.LostObjects.Where(o => o.UserIdreported == id).ToList();
            //foreach (var lo in LOlist)
            //{
            //    db.LostObjects.Remove(lo);
            //}
            InfoUser infoUser = db.InfoUsers.Find(id);
            //db.InfoUsers.Remove(infoUser);
            //db.SaveChanges();
            return RedirectToAction("Index");
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
