using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ThingsLostAndFound.Models
{
    [MetadataType(typeof(FoundObjectMetada))]
    public partial class FoundObject      //info for annotations in partial class http://alexwolfthoughts.com/adding-validation-metadata-to-entity-framework-generated-classes/
    {
        class FoundObjectMetada
        {
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public string Date { get; set; }

            [Display(Name = "Serial ID")]
            public string SerialID { get; set; }

            [Required(ErrorMessage = "Category are required")]
            public string Category { get; set; }

            [Required(ErrorMessage = "Title are required")]
            public string Title { get; set; }

            [Display(Name = "Map Location")]
            public string MapLocation { get; set; }

            [Display(Name = "Location Observations")]
            public string LocationObservations { get; set; }

            [Required(ErrorMessage = "Location are required")]
            public string Location { get; set; }

            [Display(Name = "Kind of location")]
            [Required(ErrorMessage = "Kind of location are required")]
            public string CityTownRoad { get; set; }

            //[Range(5, 100, ErrorMessage = "Movies cost between $5 and $100.")]
            //public decimal Price { get; set; }
        }
    }
}