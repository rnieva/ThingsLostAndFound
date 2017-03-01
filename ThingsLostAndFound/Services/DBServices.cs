using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Services
{
    public class DBServices:IDBServices
    {
        private TLAFEntities db = new TLAFEntities();

        public Setting GetSettings()
        {
            Setting settings = db.Settings.Find(1); // At the moment just one row in the table with id=1
            return settings;
        }
    }
}