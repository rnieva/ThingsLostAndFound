using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Security;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class FoundObjectsController : Controller
    {
        //private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor

        // GET: FoundObjects
        public ActionResult Index(int? page)
        {
            //var foundObjectsList = db.FoundObjects.Where(f => f.State == false);
            var foundObjectsList = _IDBServices.GetListFO();
            var pager = new Pager(foundObjectsList.Count(), page, 5); //number of objects per page
            var viewModel = new IndexViewModel
            {
                FoundObjectList = foundObjectsList.OrderByDescending(x => x.Date).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
            return View(viewModel);
        }

        // GET: FoundObjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //FoundObject foundObject = db.FoundObjects.Find(id);
            FoundObject foundObject = _IDBServices.GetDetailsFO(id);
            if (foundObject == null)
            {
                return HttpNotFound();
            }
            return View(foundObject);
        }

        // GET: FoundObjects/Create 
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

        // POST: FoundObjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,Country,LocationObservations,Location,CityTownRoad,Img,SecurityQuestion")] FoundObject foundObject, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                foundObject.State = false; //I assign false value, when sombody found the object, it´ll change to true value
                foundObject.ContactState = false; // It is always false when a user create a report
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
                    foundObject.Img = true;     //There is a uploaded file
                    foundObject.FileId = file.Id;
                    //db.Files.Add(file);
                    _IDBServices.AddFileFO(file);
                } else {
                    foundObject.Img = false;   // false value if there isn´t uploaded file
                    var file = new Models.File(); // foreign key never null in this case, create un empty file
                    foundObject.FileId = file.Id;
                    //db.Files.Add(file);
                    _IDBServices.AddFileFO(file);
                }
                //db.FoundObjects.Add(foundObject);
                _IDBServices.AddFO(foundObject);
                Message msg = new Message();    //new message with new object
                msg.NewMessage = true;
                msg.Message1 = "Found Object added to list";
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = 1; //  because 1 is the admin
                msg.UserIdDest = foundObject.UserIdreported; //user that found the object
                msg.subject = "Found Object";
                msg.FoundObjectId = foundObject.Id; // id object
                msg.LostObjectId = null;
                msg.emailAddressUserDontRegis = null; // only for user not registered
                //db.Messages.Add(msg);
                _IDBServices.AddMessage(msg);
                //db.SaveChanges();
                _IDBServices.SaveChanges();
                // send email
                //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
                Setting settings = _IDBServices.GetSettings();
                if (settings.NewObject == true)
                {
                    string emailBody = sendEmail.BuildBodyEmailNewFO(foundObject);
                    string emailSubject = foundObject.Id + "# Info ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                    //InfoUser infoUser = db.InfoUsers.Find(foundObject.UserIdreported);
                    InfoUser infoUser = _IDBServices.GetInfoUser(foundObject.UserIdreported);
                    string emailRecipient = infoUser.Email;
                    if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                    {
                        // After the user has created a report, always check if there is any coincidences with data in the DB 
                        return RedirectToAction("SearchMatchesInLostObject", "FindMatches", new System.Web.Routing.RouteValueDictionary(foundObject)); // I use 2RouteValueDictionary" to pass a value of this type
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("error send email");
                    }
                }
                // After the user has created a report, always check if there is any coincidences with data in the DB 
                return RedirectToAction("SearchMatchesInLostObject", "FindMatches", new System.Web.Routing.RouteValueDictionary(foundObject) ); // I use 2RouteValueDictionary" to pass a value of this type
            }
            //ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", foundObject.UserIdreported);
            ViewBag.UserIdreported = foundObject.UserIdreported;
            return View(foundObject); //If not is valid it come back to view
        }

        // GET: FoundObjects/Edit/5
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
                //FoundObject foundObject = db.FoundObjects.Find(id);
                FoundObject foundObject = _IDBServices.GetDetailsFO(id);
                if ((foundObject.UserIdreported == userId) || (roll == 1))     // This way, only the user with hus id can see his details
                {
                    if (foundObject == null)
                    {
                        return HttpNotFound();
                    }
                    //ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", foundObject.UserIdreported);  
                    return View(foundObject);
                }
                else
                {
                    return HttpNotFound();  //TODO: add view reject or error
                }
            }
            return HttpNotFound();
        }

        // POST: FoundObjects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,Country,LocationObservations,Location,CityTownRoad,State,FileId,Img,SecurityQuestion,ContactState")] FoundObject foundObject, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    //var file = db.Files.Find(foundObject.FileId);
                    var file = _IDBServices.getFile(foundObject.FileId);
                    file.FileName = System.IO.Path.GetFileName(upload.FileName);
                    file.ContentType = upload.ContentType;
                    file.FileType = System.IO.Path.GetExtension(upload.FileName);
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        file.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    foundObject.Img = true;     //There is a uploaded file
                    //db.Entry(file).State = EntityState.Modified;
                    _IDBServices.ModifiedFile(file);
                }
                else
                {
                    // keep the same img
                }
                //db.Entry(foundObject).State = EntityState.Modified;
                _IDBServices.ModifiedFO(foundObject);
                //db.SaveChanges();
                _IDBServices.SaveChanges();
                //send email
                //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
                Setting settings = _IDBServices.GetSettings();
                if (settings.EditObject == true)
                {
                    string emailBody = sendEmail.BuildBodyEmailEditObjectFO(foundObject);
                    string emailSubject = foundObject.Id + "# Edit Object ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                    //InfoUser infoUser = db.InfoUsers.Find(foundObject.UserIdreported);
                    InfoUser infoUser = _IDBServices.GetInfoUser(foundObject.UserIdreported);
                    string emailRecipient = infoUser.Email;
                    if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                    {
                        System.Diagnostics.Debug.WriteLine("email Edit Object Sent");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("error send email");
                    }
                }
                return RedirectToAction("Index");
            }
            //ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", foundObject.UserIdreported);
            //ViewBag.UserIdreported = foundObject.UserIdreported;
            return View(foundObject);
        }

        // GET: FoundObjects/Delete/5
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
                // It get user ID value from infoUserIdRolNewM
                int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
                int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
                //FoundObject foundObject = db.FoundObjects.Find(id);
                FoundObject foundObject = _IDBServices.GetDetailsFO(id);
                if ((foundObject.UserIdreported == userId) || (roll == 1))     // This way, only the user with hus id can see his details
                { 
                    if (foundObject == null)
                    {
                        return HttpNotFound();
                    }
                    return View(foundObject);
                }
                else
                {
                    return HttpNotFound();  //TODO: add view reject or error
                }
            }
            return HttpNotFound();
        }

        // POST: FoundObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int idUser,string nameContact, bool checkObjectSolved = true, bool checkUserCreateObject = true)
        {
            LostAndFoundObject lostAndFoundObject = new LostAndFoundObject();
            lostAndFoundObject.date = DateTime.Now;
            lostAndFoundObject.ObjectIDFound = id;
            lostAndFoundObject.ObjectIDLost = null;
            lostAndFoundObject.UserIdreportFound = idUser;
            if (checkObjectSolved == false)  // this is the user found the object, therefore it has had some contact between people
            {
                //InfoUser infoUser = db.InfoUsers.Where(o => o.UserName == nameContact).FirstOrDefault(); //to get the id of ContactUser
                InfoUser infoUser = _IDBServices.GetInfoUserByNameContact(nameContact);
                if (infoUser != null)
                {
                    lostAndFoundObject.UserIdreportedLost = infoUser.Id;
                    lostAndFoundObject.ContactNameuser = nameContact;
                }
                else
                {
                    lostAndFoundObject.UserIdreportedLost = null;
                    lostAndFoundObject.ContactNameuser = nameContact;
                }
                
            }
            else   // this is the user wants to forget the object 
            {
                lostAndFoundObject.UserIdreportedLost = null;
                lostAndFoundObject.ContactNameuser = null;
            }
            //db.LostAndFoundObjects.Add(lostAndFoundObject);
            _IDBServices.AddLostAndFoundObjects(lostAndFoundObject);
            // FoundObject foundObject = db.FoundObjects.Find(id);
            FoundObject foundObject = _IDBServices.GetDetailsFO(id);
            foundObject.State = true;
            //db.Entry(foundObject).State = EntityState.Modified;
            _IDBServices.ModifiedFO(foundObject);
            //Dont delete neither Files or Msgs
            //var file = db.Files.Find(foundObject.FileId);    //ADD delete the upload file if it have one
            //db.Files.Remove(file);
            //var msgListAbouthisObject = db.Messages.Where(a => a.FoundObjectId == id).ToList();
            var msgListAbouthisObject = _IDBServices.MsgListAbouthisFoundObject(id);
            //db.Messages.RemoveRange(msgListAbouthisObject);     //If the user delete the object created it will delete every msgs related to this object  //TODO: not delete messanges id fot not show    
            foreach (var msg in msgListAbouthisObject)
            {
                if (msg.ShowMsgUserId1 == null)
                {
                    msg.ShowMsgUserId1 = idUser; // REMARK: it update the value and I dont use --> db.Entry(m).State = EntityState.Modified;
                                                           //db.Entry(mgsForNotShow).State = EntityState.Modified;
                }
                else
                {   // if it entry here its because a first user delete the message before
                    msg.ShowMsgUserId2 = idUser;
                    //db.Entry(mgsForNotShow).State = EntityState.Modified;
                }
            }

            //db.FoundObjects.Remove(foundObject); // it not delete the object if not it assign as state = true
            //db.SaveChanges();
            _IDBServices.SaveChanges();
            //send email
            //Setting settings = db.Settings.Find(1); // check if newobject is true to send an email
            Setting settings = _IDBServices.GetSettings();
            if (settings.DeleteObject == true)
            {
                string emailBody = sendEmail.BuildBodyEmailDeleteObjectFO(foundObject);
                string emailSubject = foundObject.Id + "# Delete Object ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                //InfoUser infoUser = db.InfoUsers.Find(foundObject.UserIdreported);
                InfoUser infoUser = _IDBServices.GetInfoUser(foundObject.UserIdreported);
                string emailRecipient = infoUser.Email;
                if (sendEmail.sendEmailUser(emailBody, emailSubject, emailRecipient) == true)
                {
                    System.Diagnostics.Debug.WriteLine("email deleteObject Sent");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("error send email");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("send email no activate");
            }
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        protected bool CheckFile(HttpPostedFileBase upload)
        {
            int MaxContentLength = 1024 * 1024 * 5; //5 MB
            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };
           
            //foreach (var file in files)
            //{
            //    if (file == null)
            //    {
            //        return false;
            //    }
            //    else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            //    {
            //        ErrorMessage = System.Resources.ResourcesAdvert.AdvertUploadFileExtensionErrorShorter + string.Join(", ", AllowedFileExtensions);
            //        return false;
            //    }
            //    else if (file.ContentLength > MaxContentLength)
            //    {
            //        ErrorMessage = Resources.ResourcesAdvert.AdvertUploadFileTooBig + (MaxContentLength / 1024).ToString() + "MB";
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
    }
}
