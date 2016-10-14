using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThingsLostAndFound.Controllers
{
    public class MapFoundObjectsController : Controller
    {
        private TLAFEntities db = new TLAFEntities();
        // GET: MapFoundObjects
        public ActionResult Index()
        {
            return View();
        }
    }
}