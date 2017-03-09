using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class MapObjectsController : Controller
    {
        //private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor

        // GET: MapFoundObjects
        public ActionResult MapFoundObjects()
        {
            // It create a marker list with the info of each found object 
            List<InfoMarkerFoundObject> listMarkers = new List<InfoMarkerFoundObject>();
            string coordinatesFoundObject = "";
            double LatitudeT = 0.0;
            double LongitudeT = 0.0;
            //var listFoundObjects = from p in db.FoundObjects where p.State == false select p;
            var listFoundObjects = _IDBServices.GetListFO();
            foreach (var p in listFoundObjects)
            {
                coordinatesFoundObject = p.MapLocation; //Read from DB (MapLocation field, format 55.947662,-3.182259) the coordinates 
                int i = coordinatesFoundObject.IndexOf(',');
                string sub1 = coordinatesFoundObject.Substring(0, i);
                int j = (coordinatesFoundObject.Length) - (i + 1);
                string sub2 = coordinatesFoundObject.Substring(i + 1, j);
                LatitudeT = Convert.ToDouble(sub1, CultureInfo.InvariantCulture);
                LongitudeT = Convert.ToDouble(sub2, CultureInfo.InvariantCulture);
                var marker = new InfoMarkerFoundObject  //add a marker with all information about one object
                {
                    Latitude = LatitudeT,
                    Longitude = LongitudeT,
                    Id = p.Id,
                    UserIdreported = p.UserIdreported,
                    Date = p.Date,
                    Category = p.Category,
                    Brand = p.Brand,
                    Model = p.Model,
                    SerialID = p.SerialID,
                    Title = p.Title,
                    Color = p.Color,
                    Observations = p.Observations,
                    Address = p.Address,
                    ZipCode = p.ZipCode,
                    LocationObservations = p.LocationObservations,
                    Location = p.Location,
                    CityTownRoad = p.CityTownRoad,
                    SecurityQuestion = p.SecurityQuestion,
                    UserNameReport = p.InfoUser.UserName,
                    FileId = p.FileId,
                    Img = (bool)p.Img
                };
                listMarkers.Add(marker);
            }
            return View(listMarkers);   //this view show a map with found object
        }

        public ActionResult MapLostObjects()
        {
            // It create a marker list with the info of each found object 
            List<InfoMarkerLostObject> listMarkers = new List<InfoMarkerLostObject>();
            string coordinatesLostObject = "";
            double LatitudeT = 0.0;
            double LongitudeT = 0.0;
            //var listLostObjects = from p in db.LostObjects where p.State == false select p;
            var listLostObjects = _IDBServices.GetListLO();
            foreach (var p in listLostObjects)
            {
                coordinatesLostObject = p.MapLocation; //Read from DB (MapLocation field, format 55.947662,-3.182259) the coordinates 
                int i = coordinatesLostObject.IndexOf(',');
                string sub1 = coordinatesLostObject.Substring(0, i);
                int j = (coordinatesLostObject.Length) - (i + 1);
                string sub2 = coordinatesLostObject.Substring(i + 1, j);
                LatitudeT = Convert.ToDouble(sub1, CultureInfo.InvariantCulture);
                LongitudeT = Convert.ToDouble(sub2, CultureInfo.InvariantCulture);
                var marker = new InfoMarkerLostObject  //add a marker with all information about one object
                {
                    Latitude = LatitudeT,
                    Longitude = LongitudeT,
                    Id = p.Id,
                    UserIdreported = p.UserIdreported,
                    Date = p.Date,
                    Category = p.Category,
                    Brand = p.Brand,
                    Model = p.Model,
                    SerialID = p.SerialID,
                    Title = p.Title,
                    Color = p.Color,
                    Observations = p.Observations,
                    Address = p.Address,
                    ZipCode = p.ZipCode,
                    LocationObservations = p.LocationObservations,
                    Location = p.Location,
                    CityTownRoad = p.CityTownRoad,
                    UserNameReport = p.InfoUser.UserName,
                    FileId = p.FileId,
                    Img = (bool)p.Img
                };
                listMarkers.Add(marker);
            }
            return View(listMarkers);   //this view show a map with found object
        }

        //TODO: join both actions and to do just 1 action for lost and found objects

        public ActionResult MapFoundAndLostObjects(string Target)
        {
            ViewBag.Target = Target; // Target is the location into by the user in Search object, it´s to center the map in this position
            List<InfoMarkerFoundObject> listFoundMarkers = new List<InfoMarkerFoundObject>();
            List<InfoMarkerLostObject> listLostMarkers = new List<InfoMarkerLostObject>();
            List<object> listMarkers = new List<object>();
            string coordinatesObject = "";
            double LatitudeT = 0.0;
            double LongitudeT = 0.0;
            //var listLostObjects = from p in db.LostObjects where p.State == false select p;
            var listLostObjects = _IDBServices.GetListLO();
            //var listFoundObjects = from p in db.FoundObjects where p.State == false select p;
            var listFoundObjects = _IDBServices.GetListFO();
            foreach (var p in listFoundObjects)
            {
                coordinatesObject = p.MapLocation; //Read from DB (MapLocation field, format 55.947662,-3.182259) the coordinates 
                int i = coordinatesObject.IndexOf(',');
                string sub1 = coordinatesObject.Substring(0, i);
                int j = (coordinatesObject.Length) - (i + 1);
                string sub2 = coordinatesObject.Substring(i + 1, j);
                LatitudeT = Convert.ToDouble(sub1, CultureInfo.InvariantCulture);
                LongitudeT = Convert.ToDouble(sub2, CultureInfo.InvariantCulture);
                var marker = new InfoMarkerFoundObject  //add a marker with all information about one object
                {
                    Latitude = LatitudeT,
                    Longitude = LongitudeT,
                    Id = p.Id,
                    UserIdreported = p.UserIdreported,
                    Date = p.Date,
                    Category = p.Category,
                    Brand = p.Brand,
                    Model = p.Model,
                    SerialID = p.SerialID,
                    Title = p.Title,
                    Color = p.Color,
                    Observations = p.Observations,
                    Address = p.Address,
                    ZipCode = p.ZipCode,
                    LocationObservations = p.LocationObservations,
                    Location = p.Location,
                    CityTownRoad = p.CityTownRoad,
                    SecurityQuestion = p.SecurityQuestion,
                    UserNameReport = p.InfoUser.UserName,
                    FileId = p.FileId,
                    Img = (bool)p.Img
                };
                listFoundMarkers.Add(marker);
            }
            foreach (var p in listLostObjects)
            {
                coordinatesObject = p.MapLocation; //Read from DB (MapLocation field, format 55.947662,-3.182259) the coordinates 
                int i = coordinatesObject.IndexOf(',');
                string sub1 = coordinatesObject.Substring(0, i);
                int j = (coordinatesObject.Length) - (i + 1);
                string sub2 = coordinatesObject.Substring(i + 1, j);
                LatitudeT = Convert.ToDouble(sub1, CultureInfo.InvariantCulture);
                LongitudeT = Convert.ToDouble(sub2, CultureInfo.InvariantCulture);
                var marker = new InfoMarkerLostObject  //add a marker with all information about one object
                {
                    Latitude = LatitudeT,
                    Longitude = LongitudeT,
                    Id = p.Id,
                    UserIdreported = p.UserIdreported,
                    Date = p.Date,
                    Category = p.Category,
                    Brand = p.Brand,
                    Model = p.Model,
                    SerialID = p.SerialID,
                    Title = p.Title,
                    Color = p.Color,
                    Observations = p.Observations,
                    Address = p.Address,
                    ZipCode = p.ZipCode,
                    LocationObservations = p.LocationObservations,
                    Location = p.Location,
                    CityTownRoad = p.CityTownRoad,
                    UserNameReport = p.InfoUser.UserName,
                    FileId = p.FileId,
                    Img = (bool)p.Img
                };
                listLostMarkers.Add(marker);
            }
            listMarkers.Add(listFoundMarkers);
            listMarkers.Add(listLostMarkers);
            return View(listMarkers);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}