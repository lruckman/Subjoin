//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Subjoin.DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VenueCategoryId { get; set; }
        public string OSMId { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        public virtual VenueCategory Category { get; set; }
    }
}
