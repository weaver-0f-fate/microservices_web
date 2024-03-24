using Microsoft.EntityFrameworkCore;
using Subscription.Domain.Aggregates.Base;

namespace Subscription.Infrastructure;

public class SubscriptionDbContext : DbContext
{
    public DbSet<Domain.Aggregates.Subscription> Subscriptions { get; set; }

    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options)
        : base(options) { }

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