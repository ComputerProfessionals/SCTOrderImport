namespace OrderImportClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sct.TimeCodeMap")]
    public partial class TimeCodeMap
    {
        [Key]
        public int SCTTimeMapID { get; set; }

        public int? TimeCode { get; set; }

        [StringLength(20)]
        public string TimeVal { get; set; }
    }
}
