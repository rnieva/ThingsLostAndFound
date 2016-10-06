using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img,State")] FoundObject foundObject)
        {
            if (ModelState.IsValid)
            {
                db.FoundObjects.Add(foundObject);
                db.SaveChanges();
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
