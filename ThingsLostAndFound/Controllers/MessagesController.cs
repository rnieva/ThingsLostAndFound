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
        public ActionResult ShowMessages(int id)
        {
            Message msg = db.Messages.Find(id);
            ViewBag.SupportMessages = msg.SupportMessages;
            return View();
        }
    }
}