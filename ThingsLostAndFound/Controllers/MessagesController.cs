using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Security;

namespace ThingsLostAndFound.Controllers
{
    public class MessagesController : Controller //This controller to managed the messages from Support and from others users
    {
        private TLAFEntities db = new TLAFEntities();
        // GET: Messages

        public ActionResult ShowMessages(int id) // user id registered
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string infoUserIdRolNewM = ticket.UserData.ToString();
            int userId = Int32.Parse(infoUserIdRolNewM.Substring(0, infoUserIdRolNewM.IndexOf("|")));
            int roll = Int32.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("|")) + 1, infoUserIdRolNewM.IndexOf("||") - 2));
            // Only users with roll 1 and the user can read their own messages
            if ((id == userId) || (roll == 1))     // This way, only the user with hus id can see his details
            {
                //// show messages from support
                //InfoUser user = db.InfoUsers.Find(id);
                //int test = (int)user.MessagesID;
                //var supportMessagesListbyId = db.Messages.Where(a => a.Id.Equals(test)).ToList();

                //ViewBag.SupportMessages = user.Message.SupportMessages;
                //// search the messages in userContactRegistered and user contact dont register, from others users to this ID user
                //var userContactDontRegisteredListbyId = db.UsersContactDontRegisters.Where(a => a.UserIdReportFound.Equals(id)).ToList();
                //var userContactRegisteredListbyId = db.UsersContactRegistereds.Where(a => a.UserIdReportFound.Equals(id)).ToList();
                //List<object> myUsersContactList = new List<object>();
                //myUsersContactList.Add(userContactDontRegisteredListbyId);
                //myUsersContactList.Add(userContactRegisteredListbyId);
                //user.Message.NewMessage = false;
                //db.SaveChanges();

                ////update the cookie with new user data, now the newMessage is false, to change color label new Message
                //bool newMessage = bool.Parse(infoUserIdRolNewM.Substring((infoUserIdRolNewM.IndexOf("||")) + 2, ((infoUserIdRolNewM.Length) - (infoUserIdRolNewM.IndexOf("||") + 2))));
                //if (newMessage == true)
                //{
                //    infoUserIdRolNewM = infoUserIdRolNewM.Replace("True", "False");
                //    var newticket = new FormsAuthenticationTicket(ticket.Version,
                //                                                  ticket.Name,
                //                                                  ticket.IssueDate,
                //                                                  ticket.Expiration,
                //                                                  false,
                //                                                  infoUserIdRolNewM,
                //                                                  ticket.CookiePath);
                //    authCookie.Value = FormsAuthentication.Encrypt(newticket);
                //    Response.Cookies.Set(authCookie);
                //}
                //return View(myUsersContactList);
                return View();
            }
            else
            {
                return HttpNotFound();  //TODO: add view reject or error
            }
                
        }
        public ActionResult Answer(int idObj, int idRequestLost) // idObj is the Object Id, idRequestLost is the user that Lost the object
        {
            int i;
            return View();
        }
    }
}