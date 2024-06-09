using Algorithms.Domain.Core.Interfaces;
using Algorithms.Domain.Core.User;

namespace Algorithms.Domain.Providers;

public interface IUserIdProvider : IProvider<string>
{
}

public class UserIdProvider : IUserIdProvider
{
    private readonly UserContext UserContext;

    public UserIdProvider(UserContext userContext)
    {
        UserContext = userContext;
    }

    public string Get()
    {
        return UserContext.UserId;
    }
}