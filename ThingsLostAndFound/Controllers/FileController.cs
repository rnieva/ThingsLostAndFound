using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Controllers
{
    public class FileController : Controller    //This controller return a File from DB (img uploaded by user), It call from Index and Details views
    {
        private TLAFEntities db = new TLAFEntities();
        // GET: File
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.Files.Find(id);
            if (fileToRetrieve.Content != null)
            {
                return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
            }
            else
            {
                return null;
            }
            
        }
    }
}