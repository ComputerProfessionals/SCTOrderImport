﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SCT : DbContext
    {
        public SCT()
            : base("name=SCT")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<SiteConfig> SiteConfigs { get; set; }
        public DbSet<ShippingRequestDetail> ShippingRequestDetails { get; set; }
        public DbSet<ShippingRequestError> ShippingRequestErrors { get; set; }
        public DbSet<ShippingRequestHeader> ShippingRequestHeaders { get; set; }
        public DbSet<TimeCodeMap> TimeCodeMaps { get; set; }
    }
}
