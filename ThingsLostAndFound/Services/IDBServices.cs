using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Services
{
    public interface IDBServices
    {
        Setting GetSettings();
    }
}