namespace NoteKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Note")]
    public partial class Note
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Note()
        {
            Tags = new HashSet<Tag>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        private string _title;
        private string _body;
        private int _userId;
        private DateTime _creationTime;
        private DateTime _lastModifiedTime;

        [Required]
        [StringLength(50)]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Column(TypeName = "text")]
        [Required]
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public DateTime CreationTime
        {
            get { return _creationTime; }
            set { _creationTime = value; }
        }

        public DateTime LastModificationDate
        {
            get { return _lastModifiedTime; }
            set { _lastModifiedTime = value; }
        }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
