﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class LostObjectsController : Controller
    {
        private TLAFEntities db = new TLAFEntities();

        // GET: LostObjects
        public ActionResult Index()
        {
            //var lostObjects = db.LostObjects.Include(l => l.InfoUser);
            var lostObjects = db.LostObjects.Where(l => l.State == false);
            return View(lostObjects.ToList());
        }

        // GET: LostObjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LostObject lostObject = db.LostObjects.Find(id);
            if (lostObject == null)
            {
                return HttpNotFound();
            }
            return View(lostObject);
        }

        // GET: LostObjects/Create
        public ActionResult Create()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string infoUserIdRolNewM = ticket.UserData.ToString();
                // It get user ID value from infoUserIdRolNewM
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                string name = (User.Identity.Name);
                //ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName");
                ViewBag.UserIdreported = userId;
                ViewBag.UserNamereported = name;
                return View();
            }
            else
            {
                //anonymous, The user must be registered
                return View("CreateObjMe");
            }
        }

        // POST: LostObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img,State")] LostObject lostObject, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                lostObject.State = false; //I assign false value, when sombody found the object, it´ll change to true value
                lostObject.ContactState = false; // It is always false when a user create a report
                if (upload != null && upload.ContentLength > 0)
                {
                    var file = new Models.File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType,
                        FileType = System.IO.Path.GetExtension(upload.FileName)
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        file.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    lostObject.Img = true;     //There is a uploaded file
                    lostObject.FileId = file.Id;
                    db.Files.Add(file);
                }
                else
                {
                    lostObject.Img = false;   // false value if there isn´t uploaded file
                    var file = new Models.File(); // foreign key never null, create un empty file
                    lostObject.FileId = file.Id;
                    db.Files.Add(file);
                }
                db.LostObjects.Add(lostObject);
                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = "Lost Object added to list";
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = 1; //  because 1 is the admin
                msg.UserIdDest = lostObject.UserIdreported; //user that found the object
                msg.subject = "Lost Object";
                msg.FoundObjectId = null; // id object
                msg.LostObjectId = lostObject.Id;
                msg.emailAddressUserDontRegis = null; // only for user not registered
                db.Messages.Add(msg);
                db.SaveChanges();
                string emailBody = sendEmail.BuildBodyEmailNewLO(lostObject);
                string emailSubject = lostObject.Id + "# Info ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                InfoUser infoUser = db.InfoUsers.Find(lostObject.UserIdreported);
                string emailRecipient = infoUser.Email;
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    return RedirectToAction("SearchMatchesInFoundObject", "FindMatches", new System.Web.Routing.RouteValueDictionary(lostObject)); // I use 2RouteValueDictionary" to pass a value of this type
                }
                else
                {
                    //error send email
                }
                //return RedirectToAction("Index");
                // After the user has created a report, always check if there is any coincidences with data in the DB 
                return RedirectToAction("SearchMatchesInFoundObject", "FindMatches", new System.Web.Routing.RouteValueDictionary(lostObject)); // I use 2RouteValueDictionary" to pass a value of this type
            }

            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", lostObject.UserIdreported);
            return View(lostObject);
        }

        // GET: LostObjects/Edit/5
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
                // It get user ID value from infoUserIdRolNewM
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                LostObject lostObject = db.LostObjects.Find(id);
                if ((lostObject.UserIdreported == userId) || (roll == 1))     // This way, only the user with hus id can see his details
                {
                    if (lostObject == null)
                    {
                        return HttpNotFound();
                    }
                    //ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", foundObject.UserIdreported);  
                    return View(lostObject);
                }
                else
                {
                    return HttpNotFound();  //TODO: add view reject or error
                }
            }
            return HttpNotFound();
            
        }

        // POST: LostObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,State,FileId,Img,ContactState")] LostObject lostObject, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var file = db.Files.Find(lostObject.FileId);
                    file.FileName = System.IO.Path.GetFileName(upload.FileName);
                    file.ContentType = upload.ContentType;
                    file.FileType = System.IO.Path.GetExtension(upload.FileName);
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        file.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    lostObject.Img = true;     //There is a uploaded file
                    db.Entry(file).State = EntityState.Modified;
                }
                else
                {
                    // keep the same img
                }
                db.Entry(lostObject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", lostObject.UserIdreported);
            return View(lostObject);
        }

        // GET: LostObjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LostObject lostObject = db.LostObjects.Find(id);
            if (lostObject == null)
            {
                return HttpNotFound();
            }
            return View(lostObject);
        }

        // POST: LostObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int idUser, string nameContact, bool checkObjectSolved = true, bool checkUserCreateObject = true)
        {
            LostAndFoundObject lostAndFoundObject = new LostAndFoundObject();
            lostAndFoundObject.date = DateTime.Now;
            lostAndFoundObject.ObjectIDFound = null;
            lostAndFoundObject.ObjectIDLost = id;
            lostAndFoundObject.UserIdreportedLost = idUser;
            if (checkObjectSolved == false)  // this is the user lost the object
            {
                InfoUser infoUser = db.InfoUsers.Where(o => o.UserName == nameContact).FirstOrDefault();
                if (infoUser != null)
                {
                    lostAndFoundObject.UserIdreportFound = infoUser.Id;
                    lostAndFoundObject.ContactNameuser = nameContact;
                }
                else
                {
                    lostAndFoundObject.UserIdreportFound = null;
                    lostAndFoundObject.ContactNameuser = nameContact;
                }

            }
            else   // this is the user wants to forget the object 
            {
                lostAndFoundObject.UserIdreportFound = null;
                lostAndFoundObject.ContactNameuser = null;
            }
            db.LostAndFoundObjects.Add(lostAndFoundObject);
            LostObject lostObject = db.LostObjects.Find(id);
            lostObject.State = true;
            db.Entry(lostObject).State = EntityState.Modified;
            //var file = db.Files.Find(foundObject.FileId);    //ADD delete the upload file if it have one
            //db.Files.Remove(file);
            var msgListAbouthisObject = db.Messages.Where(a => a.LostObjectId == id).ToList();
            db.Messages.RemoveRange(msgListAbouthisObject);     //If the user delete the object created it will delete every msgs related to this object  //TODO: not delete messanges id fot not show    
            //foreach (var msg in msgListAbouthisObject)
            //{
            //    msg.ShowMsgUserId1 = idUser;
            //    //db.Entry(mgs).State = EntityState.Modified;
            //}
            //db.FoundObjects.Remove(foundObject); // it not delete the object if not it assign as state = true
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
