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
            var listLostObjects = from p in db.LostObjects select p;

            var LostObjectMatchesList2 = from p in db.LostObjects where p.Category == Category select p;
            var LostObjectMatchesList3 = from p in db.LostObjects where p.Category == Category && p.Location == Location select p;

            foreach (var p in listLostObjects)
            {
                if (p.Category == Category)
                {
                    switch (p.Category) // Depens category the filters are different
                    {
                        case "Animals Pets":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        case "Bags, Luggage":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        case "Clothing":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        case "Electronics":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        case "Jewelry":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        case "Toys":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        case "Tickets":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        case "Personal":
                            LostObjectMatchesList.Add(p);
                            numberResults++;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Not coincidences");
                            ViewBag.msg = "Not coincidences";
                            break;
                    }
                }
                //if (p.Category == Category)
                //{
                //    LostObjectMatchesList.Add(p);
                //    numberResults++;
                //}
            }
            ViewData["numberResults"] = numberResults;
            return View(LostObjectMatchesList2);
        }

        public ActionResult SearchMatchesInFoundObject(LostObject lostObject)
        {//If the user create a Lost Object report it will have be check in Found Object List

            return View();
        }
    }
}