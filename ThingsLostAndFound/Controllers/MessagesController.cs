using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class MessagesController : Controller //This controller to managed the messages from Support and from others users
    {
        private TLAFEntities db = new TLAFEntities();
        // GET: Messages
        public ActionResult ShowMessages(int id) // user id registered
        {
            // show messages from support
            InfoUser user = db.InfoUsers.Find(id);
            ViewBag.SupportMessages = user.Message.SupportMessages;
            // search the messages in userContactRegistered and dont register from others users to this ID user
            var userContactDontRegisteredListbyId = db.UsersContactDontRegisters.Where(a => a.UserIdReportFound.Equals(id)).ToList();
            var userContactRegisteredListbyId = db.UsersContactRegistereds.Where(a => a.UserIdReportFound.Equals(id)).ToList();
            List<object> myUsersContactList = new List<object>();
            myUsersContactList.Add(userContactDontRegisteredListbyId);
            myUsersContactList.Add(userContactRegisteredListbyId);
            user.Message.NewMessage = false;
            return View(myUsersContactList);
        }
    }
}