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
            // show messages from support
            InfoUser user = db.InfoUsers.Find(id);
            ViewBag.SupportMessages = user.Message.SupportMessages;
            user.Message.NewMessage = false;

            // show messages from others users (related with found-lost objects

            //int refContactUserRegisterObject = (int)msg.RefMessagesContactUsersRegistered;
            ////if (refContactUserRegisterObject != null)
            //{
            //    UsersContactRegistered userContactRegistered = db.UsersContactRegistereds.Find(refContactUserRegisterObject);
            //    ViewBag.UsersContactMessages = userContactRegistered.Messages;
            //}
            return View();
        }
    }
}