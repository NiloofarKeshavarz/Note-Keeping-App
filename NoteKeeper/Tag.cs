namespace NoteKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tag")]
    public partial class Tag
    {
        public Tag(){ }

        public Tag(string name)
        {
            Name = name;
        }

        private string _name;

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //Navigation Properties
        public ICollection<Note> Notes { get; set; }
    }
}
