namespace OrderImportClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sct.ShippingRequestErrors")]
    public partial class ShippingRequestError
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShippingRequestErrorID { get; set; }

        public string ErrorDetail { get; set; }

        [StringLength(4000)]
        public string Module { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
