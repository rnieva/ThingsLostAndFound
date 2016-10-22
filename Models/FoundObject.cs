//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThingsLostAndFound.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FoundObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FoundObject()
        {
            this.LostAndFoundObjects = new HashSet<LostAndFoundObject>();
        }
    
        public int Id { get; set; }
        public int UserIdreported { get; set; }
        public System.DateTime Date { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialID { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public string Observations { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string MapLocation { get; set; }
        public string LocationObservations { get; set; }
        public string Location { get; set; }
        public string CityTownRoad { get; set; }
        public Nullable<bool> Img { get; set; }
        public bool State { get; set; }
        public int FileId { get; set; }
        public string SecurityQuestion { get; set; }
        public Nullable<bool> ContactState { get; set; }
    
        public virtual InfoUser InfoUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LostAndFoundObject> LostAndFoundObjects { get; set; }
        public virtual File File { get; set; }
    }
}
