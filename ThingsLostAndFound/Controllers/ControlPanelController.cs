using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Security;

namespace ThingsLostAndFound.Controllers
{
    public class ControlPanelController : Controller  //In this controller only the admin can change soñe settings like send Messages by email
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
        public ActionResult Settings([Bind(Include = "Id,NewObject,EditObject,DeleteObject,NewUser,EditUser,DeleteUser,SendMsgFoUserNR,SendMsgFoUserReg,SendMsgLoUserNR,SendMsgLoUserReg,SendPass,ChangePass,EmailMsgs")] Setting settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(settings).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Settings");
        }

        [RoleAuthorization(Roles = "1")]
        public ActionResult Statistics()
        {
            ViewBag.FOTotal = db.FoundObjects.Count();
            ViewBag.FOsolved = db.FoundObjects.Count(p => p.State == true);
            ViewBag.FOpending = db.FoundObjects.Count(p => p.State == false);
            ViewBag.LOTotal = db.LostAndFoundObjects.Count();
            ViewBag.LOsolved = db.LostObjects.Count(p => p.State == true);
            ViewBag.LOpending = db.LostObjects.Count(p => p.State == false); 
            ViewBag.Msgs = db.Messages.Count();
            ViewBag.Files = db.Files.Count();
            ViewBag.Users = db.InfoUsers.Count();
            return View();
        }

        public ActionResult ChartFO()
        {
            int FOTotal = db.FoundObjects.Count();
            int FOsolved = db.FoundObjects.Count(p => p.State == true);
            int FOpending = db.FoundObjects.Count(p => p.State == false);
            int[] data = new int[3] { FOTotal, FOsolved, FOpending };
            string[] dataNames = new string[3] { "FOTotal", "FOsolved", "FOpending" };
            var chart = new Chart(width: 400, height: 300, theme: ChartTheme.Green)
            .AddTitle("Found Objects")
            .AddSeries(chartType: "Pie",
                            xValue: dataNames,
                            yValues: data)
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult ChartLO()
        {
            int LOTotal = db.LostObjects.Count();
            int LOsolved = db.LostObjects.Count(p => p.State == true);
            int LOpending = db.LostObjects.Count(p => p.State == false);
            int[] data = new int[3] { LOTotal, LOsolved, LOpending };
            string[] dataNames = new string[3] { "LOTotal", "LOsolved", "LOpending" };
            var chart = new Chart(width: 400, height: 300, theme: ChartTheme.Green)
            .AddTitle("Lost Objects")
            .AddSeries(chartType: "Pie",
                            xValue: dataNames,
                            yValues: data)
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }
    }
}