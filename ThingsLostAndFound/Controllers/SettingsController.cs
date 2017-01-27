using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Security;

namespace ThingsLostAndFound.Controllers
{
    public class SettingsController : Controller  //In this controller only the admin can change soñe settings like send Messages by email
    {
        private TLAFEntities db = new TLAFEntities();

        [RoleAuthorization(Roles = "1")]
        public ActionResult Settings()
        {
            Setting settings = db.Settings.Find(1); // At the moment just one row in the table with id=1
            return View(settings);
        }

        [RoleAuthorization(Roles = "1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings([Bind(Include = "Id,NewObject,EditObject,DeleteObject,NewUser,EditUser,DeleteUser")] Setting settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(settings).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Settings");
        }
    }
}