namespace OrderImportClasses
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SCTModel : DbContext
    {
        public SCTModel()
            : base("name=SCTModel")
        {
        }

        public virtual DbSet<ShippingRequestDetail> ShippingRequestDetails { get; set; }
        public virtual DbSet<ShippingRequestError> ShippingRequestErrors { get; set; }
        public virtual DbSet<ShippingRequestHeader> ShippingRequestHeaders { get; set; }
        public virtual DbSet<TimeCodeMap> TimeCodeMaps { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShippingRequestDetail>()
                .Property(e => e.MaterialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestDetail>()
                .Property(e => e.Quantity)
                .HasPrecision(10, 1);

            modelBuilder.Entity<ShippingRequestDetail>()
                .Property(e => e.UnitOfMeasure)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestDetail>()
                .Property(e => e.NetWeight)
                .HasPrecision(12, 3);

            modelBuilder.Entity<ShippingRequestDetail>()
                .Property(e => e.GrossWeight)
                .HasPrecision(12, 3);

            modelBuilder.Entity<ShippingRequestDetail>()
                .Property(e => e.Volume)
                .HasPrecision(12, 3);

            modelBuilder.Entity<ShippingRequestError>()
                .Property(e => e.ErrorDetail)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestError>()
                .Property(e => e.Module)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.CustomerPONum)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.ConsignmentRef)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.TemperatureID)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.MeansOfTransportID)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.CustomerAccountID)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.PickupFromID)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.PickupTime)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.DeliverToID)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.DeliveryTime)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.DeliveryInstructions)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .Property(e => e.SAPOrderNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingRequestHeader>()
                .HasMany(e => e.ShippingRequestDetails)
                .WithRequired(e => e.ShippingRequestHeader)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TimeCodeMap>()
                .Property(e => e.TimeVal)
                .IsUnicode(false);
        }
    }
}
