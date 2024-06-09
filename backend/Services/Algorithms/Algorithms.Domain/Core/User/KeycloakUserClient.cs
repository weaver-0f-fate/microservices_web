namespace Algorithms.Domain.Core.User;

public interface IKeycloakUserClient
{
    public Task<UserInfo> GetUserInfo(string userBearerToken);
}

public class UserInfo
{
    public IEnumerable<string> Roles { get; set; }
}
