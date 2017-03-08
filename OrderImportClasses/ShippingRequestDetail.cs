namespace OrderImportClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sct.ShippingRequestDetails")]
    public partial class ShippingRequestDetail
    {
        [Key]
        public int ShipReqDtlID { get; set; }

        public int ShipReqID { get; set; }

        [StringLength(100)]
        public string MaterialNumber { get; set; }

        public decimal Quantity { get; set; }

        [StringLength(10)]
        public string UnitOfMeasure { get; set; }

        public decimal NetWeight { get; set; }

        public decimal GrossWeight { get; set; }

        public decimal Volume { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public virtual ShippingRequestHeader ShippingRequestHeader { get; set; }
    }
}
