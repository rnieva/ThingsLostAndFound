
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
    
public partial class InfoUser
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public InfoUser()
    {

        this.FoundObjects = new HashSet<FoundObject>();

        this.LostObjects = new HashSet<LostObject>();

        this.Messages = new HashSet<Message>();

        this.Messages1 = new HashSet<Message>();

        this.LostAndFoundObjects = new HashSet<LostAndFoundObject>();

        this.LostAndFoundObjects1 = new HashSet<LostAndFoundObject>();

    }


    public int Id { get; set; }

    public string UserName { get; set; }

    public string UserPass { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public int Rol { get; set; }

    public System.DateTime Date { get; set; }

    public string UserSalt { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FoundObject> FoundObjects { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<LostObject> LostObjects { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Message> Messages { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Message> Messages1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<LostAndFoundObject> LostAndFoundObjects { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<LostAndFoundObject> LostAndFoundObjects1 { get; set; }

}

}
