using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "Event 1",
                Category = "Category 1",
                Place = "Place 1",
                Date = new DateTime(2023, 8, 15),
                UtcTime = new TimeSpan(18, 0, 0),
                Description = "Description of Event 1",
                AdditionalInfo = "Additional info for Event 1",
                ImageUrl = "https://example.com/image1.jpg"
            },
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "Event 2",
                Category = "Category 2",
                Place = "Place 2",
                Date = new DateTime(2023, 8, 20),
                UtcTime = new TimeSpan(19, 30, 0),
                Description = "Description of Event 2",
                AdditionalInfo = "Additional info for Event 2",
                ImageUrl = "https://example.com/image2.jpg"
            },
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "Event 3",
                Category = "Category 1",
                Place = "Place 3",
                Date = new DateTime(2023, 8, 25),
                UtcTime = new TimeSpan(16, 0, 0),
                Description = "Description of Event 3",
                AdditionalInfo = "Additional info for Event 3",
                ImageUrl = "https://example.com/image3.jpg"
            },
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "Event 4",
                Category = "Category 3",
                Place = "Place 4",
                Date = new DateTime(2023, 9, 5),
                UtcTime = new TimeSpan(17, 0, 0),
                Description = "Description of Event 4",
                AdditionalInfo = "Additional info for Event 4",
                ImageUrl = "https://example.com/image4.jpg"
            },
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "Event 5",
                Category = "Category 2",
                Place = "Place 5",
                Date = new DateTime(2023, 9, 10),
                UtcTime = new TimeSpan(20, 0, 0),
                Description = "Description of Event 5",
                AdditionalInfo = "Additional info for Event 5",
                ImageUrl = "https://example.com/image5.jpg"
            }
        );
    }

    public override int SaveChanges()
    {
        var entities = ChangeTracker
            .Entries()
            .Where(e => e.Entity is EntityBase && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entities)
        {
            var now = DateTimeOffset.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((EntityBase)entityEntry.Entity).CreatedAt = now;
            }

            ((EntityBase)entityEntry.Entity).UpdatedAt = now;
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries()
            .Where(e => e.Entity is EntityBase && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entities)
        {
            var now = DateTimeOffset.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((EntityBase)entityEntry.Entity).CreatedAt = now;
            }

            ((EntityBase)entityEntry.Entity).UpdatedAt = now;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}