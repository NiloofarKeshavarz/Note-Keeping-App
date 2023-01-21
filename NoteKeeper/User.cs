namespace NoteKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Notes = new HashSet<Note>();
            //Tags = new HashSet<Tag>();
        }

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        private string _userName;
        private string _password;

        [Required]
        [StringLength(50)]
        public string UserName {
            get { return _userName; }
            set { _userName = value; }
        }

        [Required]
        public string Password {
            get { return _password; }
            set { _password = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Note> Notes { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Tag> Tags { get; set; }
    }
}
