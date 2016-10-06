using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ThingsLostAndFound.Models
{
    [MetadataType(typeof(FoundObjectMetada))]
    public partial class FoundObject                //info for annotations in partial class http://alexwolfthoughts.com/adding-validation-metadata-to-entity-framework-generated-classes/
    {
        class FoundObjectMetada
        {
            [Required(ErrorMessage = "Category are required")]
            public string Category { get; set; }

            [Required(ErrorMessage = "Title are required")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Location are required")]
            public string Location { get; set; }

            [Required(ErrorMessage = "CityTownRoad are required")]
            public string CityTownRoad { get; set; }

            //[Range(5, 100, ErrorMessage = "Movies cost between $5 and $100.")]
            //public decimal Price { get; set; }
        }
    }
}