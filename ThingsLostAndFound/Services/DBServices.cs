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

        // ControlPanelController
        public Setting GetSettings()   //this function is used by FoundObjectController too
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

        //FoundObjectsController
        public List<FoundObject> GetListFO()
        {
            return db.FoundObjects.Where(f => f.State == false).ToList();
        }

        public FoundObject GetDetailsFO(int? id) 
        {
            return db.FoundObjects.Find(id);
        }

        public void AddFileFO(File file)
        {
            db.Files.Add(file);
        }

        public void AddFO(FoundObject foundObject)
        {
            db.FoundObjects.Add(foundObject);
        }

        public void AddMessage(Message msg)
        {
            db.Messages.Add(msg);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public InfoUser GetInfoUser(int? UserIdreported) // Alse use in InfoUserController
        {
            return db.InfoUsers.Find(UserIdreported);
        }

        public void ModifiedFile(File file)
        {
            db.Entry(file).State = EntityState.Modified;
        }

        public void ModifiedFO(FoundObject foundObject)
        {
            db.Entry(foundObject).State = EntityState.Modified;
        }

        public InfoUser GetInfoUserByNameContact(string nameContact)
        {
            return db.InfoUsers.Where(o => o.UserName == nameContact).FirstOrDefault(); //to get the id of ContactUser
        }
        public void AddLostAndFoundObjects(LostAndFoundObject lostAndFoundObject)
        {
            db.LostAndFoundObjects.Add(lostAndFoundObject);
        }

        public List<Message> MsgListAbouthisFoundObject(int id)
        {
            return db.Messages.Where(a => a.FoundObjectId == id).ToList();
        }

        //InfoUserController
        public void AddInfoUser(InfoUser infoUser)
        {
            db.InfoUsers.Add(infoUser);
        }

        public void ModifiedInfoUser(InfoUser infoUser)
        {
            db.Entry(infoUser).State = EntityState.Modified;
        }

        //LoginController
        public string CheckNewMessage(int id)
        {
            string newMessage = "";
            Message messageNew = new Message();
            if ((messageNew = db.Messages.Where((a => a.UserIdDest == id && a.NewMessage == true)).FirstOrDefault()) != null)
                {
                newMessage = "True"; // temp
            }
            else
            {
                newMessage = "False"; // temp
            }
            return newMessage;
        }

        //LostObjectController
        public List<LostObject> GetListLO()
        {
            return db.LostObjects.Where(f => f.State == false).ToList();
        }

        public LostObject GetDetailsLO(int? id)
        {
            return db.LostObjects.Find(id);
        }

        public void AddFileLO(File file)
        {
            db.Files.Add(file);
        }

        public void AddLO(LostObject lostObject)
        {
            db.LostObjects.Add(lostObject);
        }
        
        public void ModifiedLO(LostObject lostObject)
        {
            db.Entry(lostObject).State = EntityState.Modified;
        }

        public List<Message> MsgListAbouthisLostObject(int id)
        {
            return db.Messages.Where(a => a.LostObjectId == id).ToList();
        }

        //MessagesController
        public List<Message> GetListMessages(int id)
        {
            return db.Messages.Where(a => a.UserIdDest == id || a.UserIdSent == id).ToList();
        }

        public List<Message> GetListNewMessages(int id)
        {
            return db.Messages.Where(a => a.UserIdDest == id && a.NewMessage == true).ToList();
        }
        
        public Message GetMessage(int id)
        {
            return db.Messages.Find(id);
        }

        //ShowObjectUsersController
        public List<FoundObject> GetListFOByUser(int id)
        {
            return db.FoundObjects.Where(a => a.UserIdreported == id && a.State == false).ToList();
        }

        public List<LostObject> GetListLOByUser(int id)
        {
            return db.LostObjects.Where(a => a.UserIdreported == id && a.State == false).ToList();
        }

    }
}