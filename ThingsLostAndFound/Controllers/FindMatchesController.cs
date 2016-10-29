using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class FindMatchesController : Controller     // This controller searches the matches when a user create a new report of lost or found object
    {
        private TLAFEntities db = new TLAFEntities();
        // GET: FindMatches
        public ActionResult SearchMatchesInLostObject(FoundObject foundObject)
        {
            int Id = foundObject.Id;
            int UserIdreported = foundObject.Id;
            System.DateTime Date = foundObject.Date;
            string Category = foundObject.Category;
            string Brand = foundObject.Brand;
            string Model = foundObject.Model;
            string SerialID = foundObject.SerialID;
            string Title = foundObject.Title;
            string Color = foundObject.Color;
            string Observations = foundObject.Observations;
            string Address = foundObject.Address;
            string ZipCode = foundObject.ZipCode;
            string MapLocation = foundObject.MapLocation;
            string LocationObservations = foundObject.LocationObservations;
            string Location = foundObject.Location;
            string CityTownRoad = foundObject.CityTownRoad;
            string SecurityQuestion = foundObject.SecurityQuestion;

            int numberResults = 0;
            List<LostObject> LostObjectMatchesList = new List<LostObject>();
            var listLostObjects = from p in db.LostObjects select p;
            foreach (var p in listLostObjects)
            {
                if (p.Category == Category)
                {
                    LostObjectMatchesList.Add(p);
                    numberResults++;
                }
            }

            return View();
        }
    }
}