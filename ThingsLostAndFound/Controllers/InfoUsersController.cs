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
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, infoUserIdRolNewM.IndexOf("||") - 2));
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
                Message msg = new Message();
                msg.NewMessage = true;
                msg.MessageNumbers = 1;
                msg.SupportMessages = "Welcome to TLAF";
                db.Messages.Add(msg);
                infoUser.Rol = 3;
                infoUser.Date = DateTime.Now;
                db.InfoUsers.Add(infoUser);
                db.SaveChanges();
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
                Message msg = new Message();
                msg.NewMessage = true;
                msg.MessageNumbers = 1;
                msg.SupportMessages = "Welcome to TLAF";
                db.Messages.Add(msg);
                infoUser.Rol = 3;
                infoUser.Date = DateTime.Now;
                infoUser.MessagesID = msg.Id;
                db.InfoUsers.Add(infoUser);
                db.SaveChanges();
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
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, infoUserIdRolNewM.IndexOf("||") - 2));
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
        public ActionResult Edit([Bind(Include = "Id,UserName,UserPass,Email,PhoneNumber,Rol,Date")] InfoUser infoUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(infoUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(infoUser);
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
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, infoUserIdRolNewM.IndexOf("||") - 2));
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
            InfoUser infoUser = db.InfoUsers.Find(id);
            Message msg = db.Messages.Find(infoUser.MessagesID);
            db.Messages.Remove(msg);
            db.InfoUsers.Remove(infoUser);
            db.SaveChanges();
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
