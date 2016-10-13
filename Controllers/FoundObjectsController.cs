using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;


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
            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName");
            return View();
        }

        // POST: FoundObjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img")] FoundObject foundObject, HttpPostedFileBase upload)
        {
            foundObject.State = false; //I assign false value, when sombody found the object, it´ll change to true value
            foundObject.Img = false;   //I always always false value if there isn´t uploaded file
            if (ModelState.IsValid)
            {
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
                }
                db.FoundObjects.Add(foundObject);
                db.SaveChanges();
                //Add method to sent to user a info email
                return RedirectToAction("Index");
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
            FoundObject foundObject = db.FoundObjects.Find(id);
            if (foundObject == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", foundObject.UserIdreported);
            return View(foundObject);
        }

        // POST: FoundObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img,State")] FoundObject foundObject)
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
            FoundObject foundObject = db.FoundObjects.Find(id);
            if (foundObject == null)
            {
                return HttpNotFound();
            }
            return View(foundObject);
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
    }
}
