//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PicTick.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contest
    {
        public long Id { get; set; }
        public string ContestName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Fees { get; set; }
        public string Image { get; set; }
    }
}