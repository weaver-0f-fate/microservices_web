using Subscription.Domain.Aggregates.Base;

namespace Subscription.Domain.Aggregates;

public class Subscription : EntityBase, IAggregateRoot
{
    public User User { get; set; } = null!;
    public Guid EventUuid { get; set; }

    private Subscription() { }

    public Subscription(User user, Guid eventUuid)
    {
        Id = Guid.NewGuid();
        User = user;
        EventUuid = eventUuid;
    }
}