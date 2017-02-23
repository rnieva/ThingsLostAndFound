using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThingsLostAndFound.Models
{
    [MetadataType(typeof(SettingMetada))]
    public partial class Setting
    {
        class SettingMetada
        {
            [Display(Name = "New Object")]
            public bool NewObject { get; set; }

            [Display(Name = "Edit Object")]
            public bool EditObject { get; set; }

            [Display(Name = "Delete Object")]
            public bool DeleteObject { get; set; }

            [Display(Name = "New User")]
            public bool NewUser { get; set; }

            [Display(Name = "Edit User")]
            public bool EditUser { get; set; }

            [Display(Name = "Delete User")]
            public bool DeleteUser { get; set; }

            [Display(Name = "Change Password")]
            public bool ChangePass { get; set; }

            [Display(Name = "Send Password")]
            public bool SendPass { get; set; }

            [Display(Name = "SendMsgFoUserNR")]
            public bool SendMsgFoUserNR { get; set; }

            [Display(Name = "SendMsgFoUserReg")]
            public bool SendMsgFoUserReg { get; set; }

            [Display(Name = "SendMsgLoUserNR")]
            public bool SendMsgLoUserNR { get; set; }

            [Display(Name = "SendMsgLoUserReg")]
            public bool SendMsgLoUserReg { get; set; }

            [Display(Name = "Email Msgs between Users")]
            public bool EmailMsgs { get; set; }
        }
    }
}