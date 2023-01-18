namespace NoteKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sysdac_instances_internal
    {
        [Key]
        public Guid instance_id { get; set; }

        [Required]
        [StringLength(128)]
        public string instance_name { get; set; }

        [Required]
        [StringLength(128)]
        public string type_name { get; set; }

        [Required]
        [StringLength(64)]
        public string type_version { get; set; }

        [StringLength(4000)]
        public string description { get; set; }

        [Required]
        public byte[] type_stream { get; set; }

        public DateTime date_created { get; set; }

        [Required]
        [StringLength(128)]
        public string created_by { get; set; }
    }
}
