using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Services
{
    public class DBServices:IDBServices
    {
        private TLAFEntities db = new TLAFEntities();

        // ControlPanelCntroller
        public Setting GetSettings()
        {
            Setting settings = db.Settings.Find(1); // At the moment just one row in the table with id=1
            return settings;
        }

        public void ChangeSettings(Setting settings)
        {
            db.Entry(settings).State = EntityState.Modified;
            db.SaveChanges();
        }

        public int FoundObjectsFOTotal()
        {
            return db.FoundObjects.Count();
        }

        public int FoundObjectsFOsolved()
        {
            return db.FoundObjects.Count(p => p.State == true);
        }

        public int FoundObjectsFOpending()
        {
            return db.FoundObjects.Count(p => p.State == false);
        }

        public int LostObjectsLOTotal()
        {
            return db.LostObjects.Count();
        }

        public int LostObjectsLOsolved()
        {
            return db.LostObjects.Count(p => p.State == true);
        }

        public int LostObjectsLOpending()
        {
            return db.LostObjects.Count(p => p.State == false);
        }

        public int MessagesTotal()
        {
            return db.Messages.Count();
        }

        public int FilesTotal()
        {
            return db.Files.Count();
        }

        public int UsersTotal()
        {
            return db.InfoUsers.Count();
        }

        //FileController
        public File getFile(int id)
        {
            return db.Files.Find(id);
        }

        //FindMatchesController
        public List<LostObject> getMatchesInLO(FoundObject foundObject)
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
            string Country = foundObject.Country;

            List<LostObject> LostObjectMatchesList = new List<LostObject>();
            // check matches from much coincidences to less coincidences
            LostObjectMatchesList = (from p in db.LostObjects where p.Brand == Brand && p.SerialID == SerialID && p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.Country == Country && p.State == false select p).ToList();
            if (LostObjectMatchesList.Count == 0)
            {
                LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.State == false select p).ToList();
                if (LostObjectMatchesList.Count == 0)
                {
                    LostObjectMatchesList = (from p in db.LostObjects where p.Title == Title && p.Category == Category && p.Location == Location && p.State == false select p).ToList();
                    if (LostObjectMatchesList.Count == 0)
                    {
                        LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.Location == Location && p.State == false select p).ToList();
                        if (LostObjectMatchesList.Count == 0)
                        {
                            LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.Title == Title && p.State == false select p).ToList();
                            if (LostObjectMatchesList.Count == 0)
                            {
                                LostObjectMatchesList = (from p in db.LostObjects where p.Category == Category && p.State == false select p).ToList();
                            }
                        }
                    }
                }
            }
            return LostObjectMatchesList;
        }

        public List<FoundObject> getMatchesInFO(LostObject lostObject)
        {
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
            string Country = lostObject.Country;
            List<FoundObject> FoundObjectMatchesList = new List<FoundObject>();
            // check matches from much coincidences to less coincidences
            FoundObjectMatchesList = (from p in db.FoundObjects where p.Brand == Brand && p.SerialID == SerialID && p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.Country == Country && p.State == false select p).ToList();
            if (FoundObjectMatchesList.Count == 0)
            {
                FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Title == Title && p.Color == Color && p.CityTownRoad == CityTownRoad && p.Location == Location && p.State == false select p).ToList();
                if (FoundObjectMatchesList.Count == 0)
                {
                    FoundObjectMatchesList = (from p in db.FoundObjects where p.Title == Title && p.Category == Category && p.Location == Location && p.State == false select p).ToList();
                    if (FoundObjectMatchesList.Count == 0)
                    {
                        FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Location == Location && p.State == false select p).ToList();
                        if (FoundObjectMatchesList.Count == 0)
                        {
                            FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.Title == Title && p.State == false select p).ToList();
                            if (FoundObjectMatchesList.Count == 0)
                            {
                                FoundObjectMatchesList = (from p in db.FoundObjects where p.Category == Category && p.State == false select p).ToList();

                            }
                        }
                    }
                }
            }
            return FoundObjectMatchesList;
        }


    }
}