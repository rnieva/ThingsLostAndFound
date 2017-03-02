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

    }
}