using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class ShowObjectUsersController : Controller
    {
        private TLAFEntities db = new TLAFEntities();
        public ActionResult ShowObjects(int id) // user id registered
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string infoUserIdRolNewM = ticket.UserData.ToString();
            int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
            int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
            List<object> objectsList = new List<object>();
            int objectNumber = 0;
            if ((id == userId) || (roll == 1))     // This way, only the user with id can see
            {
                var objFoundList = db.FoundObjects.Where(a => a.UserIdreported == id).ToList();
                var objLostList = db.LostObjects.Where(a => a.UserIdreported == id).ToList();
                objectsList.Add(objFoundList);
                objectsList.Add(objLostList);
                objectNumber = objFoundList.Count + objLostList.Count;
            }
            ViewBag.objectNumber = objectNumber;
            return View(objectsList);
        }
    }
}