namespace OrderImportClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sct.ShippingRequestHeaders")]
    public partial class ShippingRequestHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ShippingRequestHeader()
        {
            ShippingRequestDetails = new HashSet<ShippingRequestDetail>();
        }

        [Key]
        public int ShipReqID { get; set; }

        [StringLength(35)]
        public string CustomerPONum { get; set; }

        [StringLength(35)]
        public string ConsignmentRef { get; set; }

        [StringLength(10)]
        public string TemperatureID { get; set; }

        [StringLength(10)]
        public string MeansOfTransportID { get; set; }

        [StringLength(10)]
        public string CustomerAccountID { get; set; }

        [StringLength(10)]
        public string PickupFromID { get; set; }

        [Column(TypeName = "date")]
        public DateTime PickupDate { get; set; }

        [StringLength(5)]
        public string PickupTime { get; set; }

        [StringLength(10)]
        public string DeliverToID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DeliveryDate { get; set; }

        [StringLength(5)]
        public string DeliveryTime { get; set; }

        public string DeliveryInstructions { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public bool ProcessedSuccessfully { get; set; }

        [StringLength(100)]
        public string SAPOrderNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShippingRequestDetail> ShippingRequestDetails { get; set; }
    }
}
