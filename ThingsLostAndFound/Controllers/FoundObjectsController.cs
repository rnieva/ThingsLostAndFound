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
        private TLAFEntities db = new TLAFEntities();

        // GET: FoundObjects
        public ActionResult Index()
        {
            var foundObjects = db.FoundObjects.Include(f => f.InfoUser);
            return View(foundObjects.ToList());
        }

        // GET: FoundObjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoundObject foundObject = db.FoundObjects.Find(id);
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
        public ActionResult Create([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img,SecurityQuestion")] FoundObject foundObject, HttpPostedFileBase upload)
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
                    db.Files.Add(file);
                } else {
                    foundObject.Img = false;   // false value if there isn´t uploaded file
                    var file = new Models.File(); // foreign key never null in this case, create un empty file
                    foundObject.FileId = file.Id;
                    db.Files.Add(file);
                }
                db.FoundObjects.Add(foundObject);
                Message msg = new Message();
                msg.NewMessage = true;
                msg.Message1 = "Found Object added to list";
                msg.dateMessage = DateTime.Now;
                msg.UserIdSent = 1; //  because 1 is the admin
                msg.UserIdDest = foundObject.UserIdreported; //user that found the object
                msg.subject = "Found Object";
                msg.FoundObjectId = foundObject.Id; // id object
                msg.LostObjectId = null;
                msg.emailAddressUserDontRegis = null; // only for user not registered
                db.Messages.Add(msg);
                db.SaveChanges();
                string emailBody = sendEmail.BuildBodyEmailNewFO(foundObject);
                string emailSubject = foundObject.Id + "# Info ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
                InfoUser infoUser = db.InfoUsers.Find(foundObject.UserIdreported);
                string emailRecipient = infoUser.Email;
                if (sendEmail.sendEmailUser(emailBody,emailSubject, emailRecipient) == true)
                {
                    return RedirectToAction("SearchMatchesInLostObject", "FindMatches", new System.Web.Routing.RouteValueDictionary(foundObject)); // I use 2RouteValueDictionary" to pass a value of this type
                }
                else
                {
                    //error send email
                }
                //return RedirectToAction("Index");
                // After the user has created a report, always check if there is any coincidences with data in the DB 
                return RedirectToAction("SearchMatchesInLostObject", "FindMatches", new System.Web.Routing.RouteValueDictionary(foundObject) ); // I use 2RouteValueDictionary" to pass a value of this type
            }

            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", foundObject.UserIdreported);
            return View(foundObject);
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
                FoundObject foundObject = db.FoundObjects.Find(id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,FileId,Img,SecurityQuestion,State")] FoundObject foundObject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foundObject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", foundObject.UserIdreported);
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
                FoundObject foundObject = db.FoundObjects.Find(id);
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
        public ActionResult DeleteConfirmed(int id)
        {
            FoundObject foundObject = db.FoundObjects.Find(id);
            var file = db.Files.Find(foundObject.FileId);    //ADD delete the upload file if it have one
            db.Files.Remove(file);
            db.FoundObjects.Remove(foundObject);
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

        protected bool sendEmailFoundObject(FoundObject foundObject)
        {
            string emailrecipient = System.Configuration.ConfigurationManager.AppSettings["testRecipientEmailCredentialvalue"];  //email recipient
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(emailrecipient));
            email.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailCredentialvalue"], "ThingsLostAndFound");  
            email.Subject = foundObject.Id +"# Info ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            email.Body = "<h2>Found Object Report:</h2>  <br>"
                        + "<b>Date:</b> " + foundObject.Date.ToShortDateString() + "<br>"
                        + "<b>Category:</b> " + foundObject.Category +"<br>"
                        + "<b>Brand:</b> " + foundObject.Brand + "<br>"
                        + "<b>Model:</b> " + foundObject.Model + "<br>"
                        + "<b>SerialID:</b> " + foundObject.SerialID + "<br>"
                        + "<b>Title:</b> " + foundObject.Title + "<br>"
                        + "<b>Color:</b> " + foundObject.Color + "<br>"
                        + "<b>Observations:</b> " + foundObject.Observations + "<br>"
                        //email.Body = foundObject.  add uploaded file
                        + "<b>Address:</b> " + foundObject.Address + "<br>"
                        + "<b>ZipCode:</b> " + foundObject.ZipCode + "<br>"
                        + "<b>Map Location:</b> " + foundObject.MapLocation + "<br>"
                        + "<b>Location Observations:</b> " + foundObject.LocationObservations + "<br>"
                        + "<b>Location:</b> " + foundObject.Location + "<br>"
                        + "<b>Kind of Location:</b> " + foundObject.CityTownRoad + "<br><br>"
                        + "Thanks for your help" + "<br>"
                        + "If anybody had lost this object, a email will send you";
            email.IsBodyHtml = true; // If =true, You must to add </Br> in the body
            email.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.live.com"; // hotmail email "smtp.live.com";
            smtp.Port = 25; // hotmail port 25;
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
    }
}
