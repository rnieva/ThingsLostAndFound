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
    
    public partial class LostAndFoundObject
    {
        public int Id { get; set; }
        public int UserIdreportedLost { get; set; }
        public int UserIdreportFound { get; set; }
        public System.DateTime date { get; set; }
        public int ObjectIDLost { get; set; }
        public int ObjectIDFound { get; set; }
    
        public virtual FoundObject FoundObject { get; set; }
        public virtual LostObject LostObject { get; set; }
    }
}