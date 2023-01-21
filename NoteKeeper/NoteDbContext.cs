using System;
using System.Data.Entity;
using System.Linq;

namespace NoteKeeper
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext()
            : base("name=NoteDbContext")
        {
        }

        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                .Property(e => e.Body)
                .IsUnicode(false);

            modelBuilder.Entity<Note>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Notes)
                .Map(m => m.ToTable("NoteTag").MapLeftKey("noteId").MapRightKey("tagId"));

            modelBuilder.Entity<User>()
                .HasMany(e => e.Notes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.Tags)
            //    .WithRequired(e => e.User)
            //    .WillCascadeOnDelete(false);
        }
    }

}