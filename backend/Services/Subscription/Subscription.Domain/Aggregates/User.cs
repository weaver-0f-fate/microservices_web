using Subscription.Domain.Aggregates.Base;

namespace Subscription.Domain.Aggregates;

public class User : EntityBase, IAggregateRoot
{
    public string Email { get; set; } = null!;

    private User() { }

    public User(string email)
    {
        Id = Guid.NewGuid();
        Email = email;
    }
}