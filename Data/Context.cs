using ManyToManyRemoveAndAddTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ManyToManyRemoveAndAddTest.Data;

public partial class Context : DbContext
{
	public Context()
	{
	}

	public Context(DbContextOptions<Context> options)
		: base(options)
	{
	}

	public virtual DbSet<Post> Posts { get; set; }
	public virtual DbSet<Tag> Tags { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlite("Filename=MyDatabase.db");
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Post>(entity =>
		{
			entity.ToTable("Post");

			entity.Property(e => e.Text).HasMaxLength(50);

			entity.Property(e => e.Title).HasMaxLength(50);

			entity.HasMany(d => d.Tags)
				.WithMany(p => p.Posts)
				.UsingEntity<Dictionary<string, object>>(
					"PostTag",
					l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PostTag_Tag"),
					r => r.HasOne<Post>().WithMany().HasForeignKey("PostId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PostTag_Post"),
					j =>
					{
						j.HasKey("PostId", "TagId");

						j.ToTable("PostTag");
					});
		});

		modelBuilder.Entity<Tag>(entity =>
		{
			entity.ToTable("Tag");

			entity.Property(e => e.Text).HasMaxLength(50);
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
