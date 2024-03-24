using System.ComponentModel.DataAnnotations;

namespace Notification.Domain.Aggregates.Base;

public abstract class EntityBase
{
    [Key] public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public EntityBase()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }
}