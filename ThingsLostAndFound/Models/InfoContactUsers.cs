using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThingsLostAndFound.Models
{
    public class InfoContactUsers
    {
        public int idUserFinder { get; set; }
        public int idUserRequest { get; set; }      // 0 if the user doesn´t have account
        public int idObject { get; set; }
        public string MessageText { get; set; }
    }
}