namespace NoteKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public class User
    {

        public User()
        {
        }

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string _userName;
        private string _password;

        [Required]
        [StringLength(50)]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        //navigation properties
        public List<Note> Notes { get; set; }
    }
}
