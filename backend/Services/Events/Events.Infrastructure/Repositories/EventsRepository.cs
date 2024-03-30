using Ardalis.Specification.EntityFrameworkCore;
using Events.Domain.Aggregates.Base;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories;

public class EventsRepository<Event>(ApplicationDbContext context) : RepositoryBase<Event>(context), IRepository<Event>
    where Event : class, IAggregateRoot
{
    public override Task<Event> AddAsync(Event entity, CancellationToken cancellationToken = new())
    {
        if(entity is EntityBase entityBase)
            entityBase.CreatedAt = DateTime.UtcNow;

        return base.AddAsync(entity, cancellationToken);
    }

    public override Task UpdateAsync(Event entity, CancellationToken cancellationToken = new())
    {
        if(entity is EntityBase entityBase)
            entityBase.UpdatedAt = DateTimeOffset.UtcNow;

        
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