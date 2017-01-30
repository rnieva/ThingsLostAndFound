using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Services
{
    public class sendEmail
    {
        public static bool sendEmailUser(string emailBody, string emailSubject, string emailRecipient)
        {
            string emailrecipient = System.Configuration.ConfigurationManager.AppSettings["testRecipientEmailCredentialvalue"];  //emailRecipient, //here go emailUserLostObject or emailUserFounObject if the user isn´t registerd or emailUserRequest if the user is registerd
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(emailrecipient));
            string emailFrom = System.Configuration.ConfigurationManager.AppSettings["emailCredentialvalue"];
            email.From = new MailAddress(emailFrom, "ThingsLostAndFound");
            email.Subject = emailSubject;
            email.Body = emailBody;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.live.com";
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            string emailCre = System.Configuration.ConfigurationManager.AppSettings["emailCredentialvalue"];
            string passCre = System.Configuration.ConfigurationManager.AppSettings["passEmailCredentialvalue"];
            smtp.Credentials = new NetworkCredential(emailCre, passCre); // email and pass user
            try
            {
                smtp.Send(email);
                email.Dispose();
                System.Diagnostics.Debug.WriteLine("Email sent");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error to sent email: " + ex.Message);
                return false;
            }
        }

        public static string BuildBodyEmailNewFO(FoundObject foundObject)
        {
            string emailBody = "";
            emailBody =
                        "Message sent: " + DateTime.Now.ToShortDateString() + "<br>"
                        + "<h2>Found Object Report:</h2>  <br>"
                        + "<b>Date:</b> " + foundObject.Date.ToShortDateString() + "<br>"
                        + "<b>Category:</b> " + foundObject.Category + "<br>"
                        + "<b>Brand:</b> " + foundObject.Brand + "<br>"
                        + "<b>Model:</b> " + foundObject.Model + "<br>"
                        + "<b>SerialID:</b> " + foundObject.SerialID + "<br>"
                        + "<b>Title:</b> " + foundObject.Title + "<br>"
                        + "<b>Color:</b> " + foundObject.Color + "<br>"
                        + "<b>Observations:</b> " + foundObject.Observations + "<br>"
                        //email.Body = foundObject.  add uploaded file
                        + "<b>Address:</b> " + foundObject.Address + "<br>"
                        + "<b>ZipCode:</b> " + foundObject.ZipCode + "<br>"
                        + "<b>Map Location:</b> " + foundObject.MapLocation + "<br>"
                        + "<b>Country:</b> " + foundObject.Country + "<br>"
                        + "<b>Location Observations:</b> " + foundObject.LocationObservations + "<br>"
                        + "<b>Location:</b> " + foundObject.Location + "<br>"
                        + "<b>Kind of Location:</b> " + foundObject.CityTownRoad + "<br><br>"
                        + "Thanks for your help" + "<br>"
                        + "If anybody had lost this object, a email will send you";
            return emailBody;
        }

        public static string BuildBodyEmailNewLO(LostObject lostObject)
        {
            string emailBody = "";
            emailBody =
                        "Message sent: " + DateTime.Now.ToShortDateString() + "<br>"
                        + "<h2>Lost Object Report:</h2>  <br>"
                        + "<b>Date:</b> " + lostObject.Date.ToShortDateString() + "<br>"
                        + "<b>Category:</b> " + lostObject.Category + "<br>"
                        + "<b>Brand:</b> " + lostObject.Brand + "<br>"
                        + "<b>Model:</b> " + lostObject.Model + "<br>"
                        + "<b>SerialID:</b> " + lostObject.SerialID + "<br>"
                        + "<b>Title:</b> " + lostObject.Title + "<br>"
                        + "<b>Color:</b> " + lostObject.Color + "<br>"
                        + "<b>Observations:</b> " + lostObject.Observations + "<br>"
                        //email.Body = foundObject.  add uploaded file
                        + "<b>Address:</b> " + lostObject.Address + "<br>"
                        + "<b>ZipCode:</b> " + lostObject.ZipCode + "<br>"
                        + "<b>Map Location:</b> " + lostObject.MapLocation + "<br>"
                        + "<b>Country</b> " + lostObject.Country + "<br>"
                        + "<b>Location Observations:</b> " + lostObject.LocationObservations + "<br>"
                        + "<b>Location:</b> " + lostObject.Location + "<br>"
                        + "<b>Kind of Location:</b> " + lostObject.CityTownRoad + "<br><br>"
                        + "Thanks for your help" + "<br>"
                        + "If anybody found this object, a email will send you";
            return emailBody;
        }

        public static string BuildBodyEmailUserLOToUserFO(string textMessage, string emailUserLostObject)
        {
            string emailBody = "";
            emailBody = "Request from:<b> " + emailUserLostObject + "</b><br>"
                       + "<b>Message:</b><br> " + textMessage + "<br>"
                       + "---------------------------------------------------------<br>"
                       + "Thanks for your help, please answer email<b> " + emailUserLostObject + "</b><br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

        public static string BuildBodyEmailUserFOToUserLO(string textMessage, string emailUserFoundObject)
        {
            string emailBody = "";
            emailBody = "Request from:<b> " + emailUserFoundObject + "</b><br>"
                       + "<b>Message:</b><br> " + textMessage + "<br>"
                       + "---------------------------------------------------------<br>"
                       + "Thanks for your help, please answer email<b> " + emailUserFoundObject + "</b><br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

        public static string BuildBodyEmailMessage(string textMessage, string nameUserSend, string emailUserSend)
        {
            string buildBodyEmail = "";
            buildBodyEmail = "Request from:<b> " + nameUserSend + " email: " + emailUserSend + "</b><br>"
                       + "<b>Message:</b><br> " + textMessage + "<br>"
                       + "---------------------------------------------------------<br>"
                       + "Thanks for your help, please answer email<b> " + emailUserSend + "</b><br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return buildBodyEmail;
        }

        public static string BuildBodyEmailDeleteObjectFO(FoundObject foundObject)
        {
            string emailBody = "";
            emailBody = "<b> Delete Object " + foundObject.Title + "</b><br>"
                       + "Delete Object<br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

        public static string BuildBodyEmailDeleteObjectLO(LostObject lostObject)
        {
            string emailBody = "";
            emailBody = "<b> Delete Object " + lostObject.Title + "</b><br>"
                       + "Delete Object<br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

        public static string BuildBodyEmailEditObjectFO(FoundObject foundObject)
        {
            string emailBody = "";
            emailBody = "<b> Edit Object " + foundObject.Title + "</b><br>"
                       + "Edit Object<br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

        public static string BuildBodyEmailEditObjectLO(LostObject lostObject)
        {
            string emailBody = "";
            emailBody = "<b> Edit Object " + lostObject.Title + "</b><br>"
                       + "Edit Object<br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

        public static string NewUser(string userName)
        {
            string emailBody = "";
            emailBody = "<b> New User Create: "+ userName +" </b><br>"
                       + "Edit Object<br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

        public static string EditUser(string userName)
        {
            string emailBody = "";
            emailBody = "<b> New User Create: " + userName + " </b><br>"
                       + "Edit Object<br>"
                       + "----------------------------------------------------------<br><br>"
                       + "Message sent: " + DateTime.Now.ToShortDateString();
            return emailBody;
        }

    }
}