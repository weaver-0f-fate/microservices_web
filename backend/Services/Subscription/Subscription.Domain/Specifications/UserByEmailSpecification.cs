using Ardalis.Specification;
using Subscription.Domain.Aggregates;

namespace Subscription.Domain.Specifications;

public sealed class UserByEmailSpecification : SingleResultSpecification<User>
{
    public UserByEmailSpecification(string email)
    {
        Query.Where(user => user.Email == email);
    }
}