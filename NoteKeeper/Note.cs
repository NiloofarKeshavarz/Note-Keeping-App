namespace NoteKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Note")]
    public class Note
    {
        public Note()
        {
        }

        public Note(string title, string body, DateTime creationTime, DateTime lastModificationDate, int userId)
        {
            Title = title;
            Body = body;
            CreationTime = creationTime;
            LastModificationDate = lastModificationDate;
            UserId = userId;
        }

        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public int UserId { get; set; }
        public User User { get; set; }

        //Navigation Properties
        public ICollection<Tag> Tags { get; set; }
    }
}
