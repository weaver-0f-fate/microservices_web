using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Subscription.Domain.Aggregates.Base;

namespace Subscription.Infrastructure.Repositories;

public class SubscriptionsRepository<Subscription> : RepositoryBase<Subscription>, IRepository<Subscription> where Subscription : class, IAggregateRoot
{
    private readonly SubscriptionDbContext _context;

    public SubscriptionsRepository(SubscriptionDbContext context) : base(context)
    {
        _context = context;
    }

    public override Task<Subscription> AddAsync(Subscription entity, CancellationToken cancellationToken = new CancellationToken())
    {
        if (entity is EntityBase entityBase)
            entityBase.CreatedAt = DateTime.UtcNow;

        return base.AddAsync(entity, cancellationToken);
    }

    public override Task UpdateAsync(Subscription entity, CancellationToken cancellationToken = new CancellationToken())
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
        foreach (var entry in _context.ChangeTracker.Entries())
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