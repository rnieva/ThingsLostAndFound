using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThingsLostAndFound.Models
{
    [MetadataType(typeof(InfoUserMetada))]
    public partial class InfoUser
    {
        class InfoUserMetada
        {
            [Display(Name = "User Name")]
            [Required(ErrorMessage = "UserName is required")]
            public string UserName { get; set; }

            [Display(Name = "User Password")]
            [MinLength(4)]
            [Required(ErrorMessage = "UserPass is required")]
            public string UserPass { get; set; }

            [Display(Name = "User Email")]
            [Required(ErrorMessage= "Email is required")]
            public string Email { get; set; }

        }
    }
}