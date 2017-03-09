using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class ShowObjectUsersController : Controller  
    {
        //private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor

        public ActionResult ShowObjects(int id, int? page) // user id registered , show the objects created by the user
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string infoUserIdRolNewM = ticket.UserData.ToString();
            int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
            int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
            int objectNumber = 0;
            List<object> objectsList = new List<object>();
            if ((id == userId) || (roll == 1))     // This way, only the user with id can see
            {
                //var objFoundList = db.FoundObjects.Where(a => a.UserIdreported == id).ToList();
                var objFoundList = _IDBServices.GetListFOByUser(id);
                //var objLostList = db.LostObjects.Where(a => a.UserIdreported == id).ToList();
                var objLostList = _IDBServices.GetListLOByUser(id);
                objectsList.Add(objFoundList);
                objectsList.Add(objLostList);
                objectNumber = objFoundList.Count + objLostList.Count;
            }
            ViewBag.objectNumber = objectNumber;
            return View(objectsList);
        }
    }
}