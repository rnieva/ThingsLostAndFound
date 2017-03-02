using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class FindMatchesController : Controller     // This controller searches the matches when a user create a new report of lost or found object
    {
        //private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor

        public ActionResult SearchMatchesInLostObject(FoundObject foundObject)
        {   //If the user create a Found Object report it will have be check in Lost Object List
            //int Id = foundObject.Id;
            //int UserIdreported = foundObject.Id;
            //System.DateTime Date = foundObject.Date;
            //string Category = foundObject.Category;
            //string Brand = foundObject.Brand;
            //string Model = foundObject.Model;
            //string SerialID = foundObject.SerialID;
            //string Title = foundObject.Title;
            //string Color = foundObject.Color;
            //string Observations = foundObject.Observations;
            //string Address = foundObject.Address;
            //string ZipCode = foundObject.ZipCode;
            //string MapLocation = foundObject.MapLocation;
            //string LocationObservations = foundObject.LocationObservations;
            //string Location = foundObject.Location;
            //string CityTownRoad = foundObject.CityTownRoad;
            //string SecurityQuestion = foundObject.SecurityQuestion;
            //string Country = foundObject.Country;
            List<LostObject> LostObjectMatchesList = new List<LostObject>();
            //// check matches from much coincidences to less coincidences
            //LostObjectMatchesList = (from p in db.LostObjects where p.Brand == Brand && p.SerialID == SerialID && p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.Country == Country && p.State == false select p).ToList();
            //if (LostObjectMatchesList.Count == 0)
            //{
            //    LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.State == false select p).ToList();
            //    if (LostObjectMatchesList.Count == 0)
            //    {
            //        LostObjectMatchesList = (from p in db.LostObjects where p.Title == Title && p.Category == Category && p.Location == Location && p.State == false select p).ToList();
            //        if (LostObjectMatchesList.Count == 0)
            //        {
            //            LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category  && p.Location == Location && p.State == false select p).ToList();
            //            if (LostObjectMatchesList.Count == 0)
            //            {
            //                LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.Title == Title && p.State == false select p).ToList();
            //                if (LostObjectMatchesList.Count == 0)
            //                {
            //                    LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.State == false select p).ToList();
            //                }
            //            }
            //        }
            //    }
            //}
            LostObjectMatchesList = _IDBServices.getMatchesInLO(foundObject);
            int numberResults = 0;
            numberResults = LostObjectMatchesList.Count;
            ViewData["numberResults"] = numberResults;
            return View(LostObjectMatchesList);
        }

        public ActionResult SearchMatchesInFoundObject(LostObject lostObject)
        {//If the user create a Lost Object report it will have be check in Found Object List
         //int Id = lostObject.Id;
         //int UserIdreported = lostObject.Id;
         //System.DateTime Date = lostObject.Date;
         //string Category = lostObject.Category;
         //string Brand = lostObject.Brand;
         //string Model = lostObject.Model;
         //string SerialID = lostObject.SerialID;
         //string Title = lostObject.Title;
         //string Color = lostObject.Color;
         //string Observations = lostObject.Observations;
         //string Address = lostObject.Address;
         //string ZipCode = lostObject.ZipCode;
         //string MapLocation = lostObject.MapLocation;
         //string LocationObservations = lostObject.LocationObservations;
         //string Location = lostObject.Location;
         //string CityTownRoad = lostObject.CityTownRoad;
         //string Country = lostObject.Country;


            List<FoundObject> FoundObjectMatchesList = new List<FoundObject>();
            //// check matches from much coincidences to less coincidences
            //FoundObjectMatchesList = (from p in db.FoundObjects where p.Brand == Brand && p.SerialID == SerialID && p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.Country == Country && p.State == false select p).ToList();
            //if (FoundObjectMatchesList.Count == 0)
            //{
            //    FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.State == false select p).ToList();
            //    if (FoundObjectMatchesList.Count == 0)
            //    {
            //        FoundObjectMatchesList = (from p in db.FoundObjects where p.Title == Title && p.Category == Category && p.Location == Location && p.State == false select p).ToList();
            //        if (FoundObjectMatchesList.Count == 0)
            //        {
            //            FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Location == Location && p.State == false select p).ToList();
            //            if (FoundObjectMatchesList.Count == 0)
            //            {
            //                FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Title == Title && p.State == false select p).ToList();
            //                if (FoundObjectMatchesList.Count == 0)
            //                {
            //                    FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.State == false select p).ToList();

            //                }
            //            }
            //        }
            //    }
            //}
            FoundObjectMatchesList = _IDBServices.getMatchesInFO(lostObject);
            int numberResults = 0;
            numberResults = FoundObjectMatchesList.Count;
            ViewData["numberResults"] = numberResults;
            return View(FoundObjectMatchesList);
        }

        // GET 
        public ActionResult SearchFoundOrLostObject(string select)
        {
            ViewBag.select = select;
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchFoundOrLostObject([Bind(Include = "Id,UserIdreported,Date,Category,Brand,Model,SerialID,Title,Color,Observations,Address,ZipCode,MapLocation,LocationObservations,Location,CityTownRoad,Img,SecurityQuestion,Country")] FoundObject foundObject, string TypeObject)
        { // like model of object I use "Found Object Model" in the form, but I use these data for both -  Found Object and Lost Object
            if (TypeObject == "FoundObject")
            {
                return RedirectToAction("SearchMatchesInLostObject", foundObject);   //A Found Object will be checked in Lost Object List 
            }
            else
            {
                // Created a temp Lost Object from data form of "Found object"
                LostObject lostObject = new LostObject();
                lostObject.Date= foundObject.Date;
                lostObject.Category = foundObject.Category;
                lostObject.Brand = foundObject.Brand;
                lostObject.Model = foundObject.Model;
                lostObject.SerialID = foundObject.SerialID;
                lostObject.Title = foundObject.Title;
                lostObject.Color = foundObject.Color;
                lostObject.Observations = foundObject.Observations;
                lostObject.Address = foundObject.Address;
                lostObject.ZipCode = foundObject.ZipCode;
                lostObject.MapLocation = foundObject.MapLocation;
                lostObject.LocationObservations = foundObject.LocationObservations;
                lostObject.Location = foundObject.Location;
                lostObject.CityTownRoad = foundObject.CityTownRoad;
                lostObject.Country = foundObject.Country;
                return RedirectToAction("SearchMatchesInFoundObject", lostObject);
            }
            
        }
    }
}