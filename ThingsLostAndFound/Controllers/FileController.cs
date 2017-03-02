using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;
using ThingsLostAndFound.Services;

namespace ThingsLostAndFound.Controllers
{
    public class FileController : Controller    //This controller return a File from DB (img uploaded by user), It call from Index and Details views
    {
        //private TLAFEntities db = new TLAFEntities();
        private readonly IDBServices _IDBServices = new DBServices(); //or I can use a constructor

        // GET: File
        public ActionResult Index(int id)
        {
            ////var fileToRetrieve = db.Files.Find(id);
            var fileToRetrieve = _IDBServices.getFile(id);
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