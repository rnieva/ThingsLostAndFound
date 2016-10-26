using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class UsersContactController : Controller
    {
        private TLAFEntities db = new TLAFEntities();
        // GET: UsersContact
        public ActionResult ContactUserFoundObject(int id, string title, string userName) //When a user has found a object and see it in the Found Objects List o Found Object Map uses this method for contact with user that found the object
        {
            //This info cames from listFO, It show in the form
            ViewBag.idObject = id;
            ViewBag.titleObject = title;
            ViewBag.userName = userName;
            return View();    //show a form to do a request to userFinder
        }

        [HttpPost]
        public ActionResult SendRequestUser(int id, string TextMessage)  //send an email with the request user
        {
            //with id from object, search the user that found the object and send him a email
            var foundObject = db.FoundObjects.Find(id);
            string SecurityQuestion = foundObject.SecurityQuestion;
            int userIdReport = foundObject.UserIdreported;
            var infouser = db.InfoUsers.Find(userIdReport);
            string nameUserFounfObject = infouser.UserName;
            string emailUserFoundObject = infouser.Email;


            InfoContactUsers infoContactUser = new InfoContactUsers();
            infoContactUser.idUserFinder = userIdReport;
            infoContactUser.idUserRequest = 0;  // check if the user that send tha request is login if not is 0
            infoContactUser.idObject = id;
            infoContactUser.MessageText = TextMessage;
 
            //sendEmailToUserThatFoundTheObject();
            return View();  // show the view successfull if the email was sended 
        }

        public void PrepareEmail()
        {

        }
        //protected bool sendEmailToUserThatFoundTheObject()
        //{
        //    string emailrecipient = System.Configuration.ConfigurationManager.AppSettings["testRecipientEmailCredentialvalue"];  //email recipient
        //    MailMessage email = new MailMessage();
        //    email.To.Add(new MailAddress(emailrecipient));
        //    email.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailCredentialvalue"], "ThingsLostAndFound");
        //    email.Subject = foundObject.Id + "# Info ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
        //    email.Body = "<h2>Found Object Report:</h2>  <br>"
        //                + "<b>Date:</b> " + foundObject.Date.ToShortDateString() + "<br>"
        //                + "<b>Category:</b> " + foundObject.Category + "<br>"
        //                + "<b>Brand:</b> " + foundObject.Brand + "<br>"
        //                + "<b>Model:</b> " + foundObject.Model + "<br>"
        //                + "<b>SerialID:</b> " + foundObject.SerialID + "<br>"
        //                + "<b>Title:</b> " + foundObject.Title + "<br>"
        //                + "<b>Color:</b> " + foundObject.Color + "<br>"
        //                + "<b>Observations:</b> " + foundObject.Observations + "<br>"
        //                //email.Body = foundObject.  add uploaded file
        //                + "<b>Address:</b> " + foundObject.Address + "<br>"
        //                + "<b>ZipCode:</b> " + foundObject.ZipCode + "<br>"
        //                + "<b>Map Location:</b> " + foundObject.MapLocation + "<br>"
        //                + "<b>Location Observations:</b> " + foundObject.LocationObservations + "<br>"
        //                + "<b>Location:</b> " + foundObject.Location + "<br>"
        //                + "<b>Kind of Location:</b> " + foundObject.CityTownRoad + "<br><br>"
        //                + "Thanks for your help" + "<br>"
        //                + "If anybody had lost this object, a email will send you";
        //    email.IsBodyHtml = true; // If =true, You must to add </Br> in the body
        //    email.Priority = MailPriority.Normal;
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp.live.com";
        //    smtp.Port = 25;
        //    smtp.EnableSsl = true;
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["emailCredentialvalue"], System.Configuration.ConfigurationManager.AppSettings["passEmailCredentialvalue"]); // email and pass user
        //    try
        //    {
        //        smtp.Send(email);
        //        email.Dispose();
        //        System.Diagnostics.Debug.WriteLine("Email sent");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Error to sent email: " + ex.Message);
        //        return false;
        //    }

        //}
    }
}