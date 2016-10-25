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
    public class LostObjectsController : Controller
    {
        private TLAFEntities db = new TLAFEntities();

        // GET: LostObjects
        public ActionResult Index()
        {
            var lostObjects = db.LostObjects.Include(l => l.InfoUser);
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
            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName");
            return View();
        }

        // POST: LostObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img,State")] LostObject lostObject)
        {
            if (ModelState.IsValid)
            {
                db.LostObjects.Add(lostObject);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            LostObject lostObject = db.LostObjects.Find(id);
            if (lostObject == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserIdreported = new SelectList(db.InfoUsers, "Id", "UserName", lostObject.UserIdreported);
            return View(lostObject);
        }

        // POST: LostObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img,State")] LostObject lostObject)
        {
            if (ModelState.IsValid)
            {
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
        public ActionResult DeleteConfirmed(int id)
        {
            LostObject lostObject = db.LostObjects.Find(id);
            db.LostObjects.Remove(lostObject);
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
