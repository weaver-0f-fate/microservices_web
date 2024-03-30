using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Subscription.Domain.Aggregates.Base;

namespace Subscription.Infrastructure.Repositories;

public class SubscriptionsRepository<Subscription>(SubscriptionDbContext context) : RepositoryBase<Subscription>(context), IRepository<Subscription>
    where Subscription : class, IAggregateRoot
{
    public override Task<Subscription> AddAsync(Subscription entity, CancellationToken cancellationToken = new())
    {
        if (entity is EntityBase entityBase)
            entityBase.CreatedAt = DateTime.UtcNow;

        return base.AddAsync(entity, cancellationToken);
    }

    public override Task UpdateAsync(Subscription entity, CancellationToken cancellationToken = new())
    {
        if (entity is EntityBase entityBase)
            entityBase.UpdatedAt = DateTime.UtcNow;

        return base.UpdateAsync(entity, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetCreatedAtAndUpdatedAtFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreatedAtAndUpdatedAtFields()
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is not EntityBase entityBase)
                continue;

            var now = DateTimeOffset.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entityBase.CreatedAt = now;
            }

            entityBase.UpdatedAt = now;
        }
    }
}