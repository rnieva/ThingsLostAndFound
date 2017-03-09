using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Security;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class MessagesController : Controller //This controller get and show the messages from Support and from others users
    {
        //private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor
        // GET: Messages

        public ActionResult ShowMessages(int id, int? page) // user id registered
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string infoUserIdRolNewM = ticket.UserData.ToString();
            int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
            int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, (infoUserIdRolNewM.IndexOf("||") - infoUserIdRolNewM.IndexOf("|") - 1)));
            // Only users with roll 1 and the specific user can read their own messages
            if ((id == userId) || (roll == 1))     // This way, only the user with id can see
            {
                // search messages from or to ID user 
                //var msgsUsersList = db.Messages.Where(a => a.UserIdDest == id || a.UserIdSent == id).ToList();
                var msgsUsersList = _IDBServices.GetListMessages(id);
                foreach (var m in msgsUsersList.Reverse<Message>())  // Reverse iteration to remove items in a foreach, don´t show msgs if the user removed it
                {
                    if ((m.ShowMsgUserId1 == id) || (m.ShowMsgUserId2 == id))
                    {
                        msgsUsersList.Remove(m);
                    }
                }
                List<object> messagesViewList = new List<object>(); // I already don´t use this
                messagesViewList.Add(msgsUsersList);
                // search new messages to show the message in red color
                //var newMessagesFlagList = db.Messages.Where(a => a.UserIdDest == id && a.NewMessage == true).ToList();
                var newMessagesFlagList = _IDBServices.GetListNewMessages(id);
                List<int> idNewmsgs = new List<int>();
                foreach (var m in newMessagesFlagList)
                {
                    m.NewMessage = false;    // REMARK: it update the value NewMessage and I dont use --> db.Entry(m).State = EntityState.Modified;
                    //db.Entry(m).State = EntityState.Modified;
                    idNewmsgs.Add(m.Id);    // store the value id of new messanges
                }
                //db.SaveChanges();
                _IDBServices.SaveChanges();
                messagesViewList.Add(idNewmsgs);
                //update the cookie with new user data, now the newMessage is false, to change color label new Message
                bool newMessage = bool.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("||")) + 2, ((infoUserIdRolNewM.Length) - (infoUserIdRolNewM.IndexOf("||") + 2))));
                if (newMessage == true)
                {
                    infoUserIdRolNewM = infoUserIdRolNewM.Replace("True", "False");
                    var newticket = new FormsAuthenticationTicket(ticket.Version,
                                                                  ticket.Name,
                                                                  ticket.IssueDate,
                                                                  ticket.Expiration,
                                                                  false,
                                                                  infoUserIdRolNewM,
                                                                  ticket.CookiePath);
                    authCookie.Value = FormsAuthentication.Encrypt(newticket);
                    Response.Cookies.Set(authCookie);
                }
                
                var pager = new Pager(msgsUsersList.Count(), page, 10); //10 is the numner of messages per page
                var viewModel = new IndexViewModel
                {
                    MessagesList = msgsUsersList.OrderByDescending(x => x.dateMessage).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    IdNewmsgs = idNewmsgs,
                    Pager = pager
                };
                ViewBag.id = id;
                return View(viewModel);

                //return View(messagesViewList); //before the add "Pagination2" it sent to View a list<Objects> with two lists
            }
            else
            {
                return HttpNotFound();  //TODO: add view reject or error
            }

        }

        public ActionResult DeleteMessages(int id) // id Message to delete
        {
            //delete Message
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string infoUserIdRolNewM = ticket.UserData.ToString();
            int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
            //add the userID to dont show the message
            Message mgsForNotShow = new Message();
            //mgsForNotShow = db.Messages.Find(id);
            mgsForNotShow = _IDBServices.GetMessage(id);
            if (mgsForNotShow.ShowMsgUserId1 == null)
            {
                mgsForNotShow.ShowMsgUserId1 = userId; // REMARK: it update the value and I dont use --> db.Entry(m).State = EntityState.Modified;
                //db.Entry(mgsForNotShow).State = EntityState.Modified;
            }
            else
            {   // if it entry here its because a first user delete the message before
                mgsForNotShow.ShowMsgUserId2 = userId;
                //db.Entry(mgsForNotShow).State = EntityState.Modified;
            }
            //db.SaveChanges();
            _IDBServices.SaveChanges();
            return RedirectToAction("ShowMessages", new { id = userId }); // add id user
        }
    }
}