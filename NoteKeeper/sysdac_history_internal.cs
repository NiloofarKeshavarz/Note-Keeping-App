namespace NoteKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sysdac_history_internal
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int action_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int sequence_id { get; set; }

        public Guid instance_id { get; set; }

        public byte action_type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(19)]
        public string action_type_name { get; set; }

        public byte dac_object_type { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(8)]
        public string dac_object_type_name { get; set; }

        public byte action_status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(11)]
        public string action_status_name { get; set; }

        public bool? required { get; set; }

        [Required]
        [StringLength(128)]
        public string dac_object_name_pretran { get; set; }

        [Required]
        [StringLength(128)]
        public string dac_object_name_posttran { get; set; }

        public string sqlscript { get; set; }

        public byte[] payload { get; set; }

        [Required]
        public string comments { get; set; }

        public string error_string { get; set; }

        [Required]
        [StringLength(128)]
        public string created_by { get; set; }

        public DateTime date_created { get; set; }

        public DateTime date_modified { get; set; }
    }
}
