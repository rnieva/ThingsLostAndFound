using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class MapFoundObjectsController : Controller
    {
        private TLAFEntities db = new TLAFEntities();

        // GET: MapFoundObjects
        public ActionResult Index()
        {
            // It create a marker list with the info of each found object 
            List<InfoMarkerFoundObject> listMarkers = new List<InfoMarkerFoundObject>();
            string coordinatesFoundObject = "";
            double LatitudeT = 0.0;
            double LongitudeT = 0.0;
            var listFoundObjects = from p in db.FoundObjects select p;
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
                };
                listMarkers.Add(marker);
            }
            return View(listMarkers);   //this view show a map with found object
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