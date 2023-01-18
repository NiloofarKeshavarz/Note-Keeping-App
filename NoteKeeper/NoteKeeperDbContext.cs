using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NoteKeeper
{
	public partial class NoteKeeperDbContext : DbContext
	{
		public NoteKeeperDbContext()
			: base("name=NoteKeeperDbContext")
		{
		}

		public virtual DbSet<sysdac_history_internal> sysdac_history_internal { get; set; }
		public virtual DbSet<sysdac_instances_internal> sysdac_instances_internal { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<sysdac_history_internal>()
				.Property(e => e.action_type_name)
				.IsUnicode(false);

			modelBuilder.Entity<sysdac_history_internal>()
				.Property(e => e.dac_object_type_name)
				.IsUnicode(false);

			modelBuilder.Entity<sysdac_history_internal>()
				.Property(e => e.action_status_name)
				.IsUnicode(false);

			modelBuilder.Entity<sysdac_history_internal>()
				.Property(e => e.comments)
				.IsUnicode(false);
		}
	}
}
