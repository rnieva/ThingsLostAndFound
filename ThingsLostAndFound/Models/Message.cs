
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
    
public partial class Message
{

    public int Id { get; set; }

    public Nullable<bool> NewMessage { get; set; }

    public string Message1 { get; set; }

    public Nullable<int> UserIdSent { get; set; }

    public Nullable<int> UserIdDest { get; set; }

    public string subject { get; set; }

    public Nullable<int> FoundObjectId { get; set; }

    public Nullable<System.DateTime> dateMessage { get; set; }

    public string emailAddressUserDontRegis { get; set; }

    public Nullable<int> LostObjectId { get; set; }

    public Nullable<int> ShowMsgUserId1 { get; set; }

    public Nullable<int> ShowMsgUserId2 { get; set; }



    public virtual FoundObject FoundObject { get; set; }

    public virtual InfoUser InfoUser { get; set; }

    public virtual InfoUser InfoUser1 { get; set; }

    public virtual LostObject LostObject { get; set; }

}

}
