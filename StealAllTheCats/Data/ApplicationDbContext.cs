using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Entities;

namespace StealAllTheCats.Data;

/// <summary>
/// The Entity Framework Core database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the cats table.
    /// </summary>
    public DbSet<CatEntity> Cats => Set<CatEntity>();

    /// <summary>
    /// Gets the tags table.
    /// </summary>
    public DbSet<TagEntity> Tags => Set<TagEntity>();

    /// <summary>
    /// Configures the entity relationships and schema.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatEntity>()
            .HasMany(c => c.Tags)
            .WithMany(t => t.Cats);
    }
}
