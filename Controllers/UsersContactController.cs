using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ThingsLostAndFound.Controllers
{
    public class UsersContactController : Controller
    {
        // GET: UsersContact
        public ActionResult ContactUserFoundObject(int id) //When a user has found a object and see it in the Found Objects List o Found Object Map uses this method for contact with user that found the object
        {
            //with id, search the user that found the object and send him a email
            //sendEmailToUserThatFoundTheObject();
            return View();
        }

        //protected bool sendEmailToUserThatFoundTheObject()
        //{
        //    string emailrecipient = "recipient";  //email recipient
        //    MailMessage email = new MailMessage();
        //    email.To.Add(new MailAddress(emailrecipient));
        //    email.From = new MailAddress("recipient", "ThingsLostAndFound");  //recipient
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
        //    smtp.Credentials = new NetworkCredential("email", "pass"); // email and pass user
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