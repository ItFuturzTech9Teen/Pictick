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
    
    public partial class Photographer
    {
        public long Id { get; set; }
        public Nullable<long> StudioId { get; set; }
        public string Name { get; set; }
        public string ReferalCode { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public Nullable<long> BranchId { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsVerified { get; set; }
        public string FCMToken { get; set; }
    }
}
