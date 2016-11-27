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
            user.Message.NewMessage = false;

            // show messages from others users (related with found-lost objects
            var testuserContactDontRegisteredListbyId = db.UsersContactDontRegisters.Where(a => a.UserIdReportFound.Equals(id)).ToList();

            // almacenar en un array - los datos de cada conversacion y agruparlos por objeto


            var userLislt2 = db.UsersContactDontRegisters.ToList();
            var userList = from p in db.UsersContactDontRegisters select p;
            foreach (var p in userList)
            {
                if (p.UserIdReportFound == id)
                {
                    ViewBag.UsersContactMessages = p.Message1;
                }

            }
            return View();
        }
    }
}