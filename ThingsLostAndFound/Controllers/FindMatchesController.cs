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
        {   //If the user create a Found Object report it will have be check in Lost Object List
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
            // check matches from much coincidences to less coincidences
            LostObjectMatchesList = (from p in db.LostObjects where p.Brand == Brand && p.SerialID == SerialID && p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location select p).ToList();
            if (LostObjectMatchesList.Count == 0)
            {
                LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location select p).ToList();
                if (LostObjectMatchesList.Count == 0)
                {
                    LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.Location == Location select p).ToList();
                    if (LostObjectMatchesList.Count == 0)
                    {
                        LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category select p).ToList();
                    }
                }
            }
            numberResults = LostObjectMatchesList.Count;
            ViewData["numberResults"] = numberResults;
            return View(LostObjectMatchesList);
        }

        public ActionResult SearchMatchesInFoundObject(LostObject lostObject)
        {//If the user create a Lost Object report it will have be check in Found Object List
            int Id = lostObject.Id;
            int UserIdreported = lostObject.Id;
            System.DateTime Date = lostObject.Date;
            string Category = lostObject.Category;
            string Brand = lostObject.Brand;
            string Model = lostObject.Model;
            string SerialID = lostObject.SerialID;
            string Title = lostObject.Title;
            string Color = lostObject.Color;
            string Observations = lostObject.Observations;
            string Address = lostObject.Address;
            string ZipCode = lostObject.ZipCode;
            string MapLocation = lostObject.MapLocation;
            string LocationObservations = lostObject.LocationObservations;
            string Location = lostObject.Location;
            string CityTownRoad = lostObject.CityTownRoad;

            int numberResults = 0;
            List<FoundObject> FoundObjectMatchesList = new List<FoundObject>();
            // check matches from much coincidences to less coincidences
            FoundObjectMatchesList = (from p in db.FoundObjects where p.Brand == Brand && p.SerialID == SerialID && p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location select p).ToList();
            if (FoundObjectMatchesList.Count == 0)
            {
                FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location select p).ToList();
                if (FoundObjectMatchesList.Count == 0)
                {
                    FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Location == Location select p).ToList();
                    if (FoundObjectMatchesList.Count == 0)
                    {
                        FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category select p).ToList();
                    }
                }
            }
            numberResults = FoundObjectMatchesList.Count;
            ViewData["numberResults"] = numberResults;
            return View(FoundObjectMatchesList);
        }
    }
}