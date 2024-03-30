using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Aggregates.Base;

namespace Notification.Infrastructure.Repositories;

public class NotificationsRepository<Notification>(NotificationDbContext context) : RepositoryBase<Notification>(context), IRepository<Notification>
    where Notification : class, IAggregateRoot
{
    public override Task<Notification> AddAsync(Notification entity, CancellationToken cancellationToken = new CancellationToken())
    {
        if (entity is EntityBase entityBase)
            entityBase.CreatedAt = DateTime.UtcNow;

        return base.AddAsync(entity, cancellationToken);
    }

    public override Task UpdateAsync(Notification entity, CancellationToken cancellationToken = new CancellationToken())
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