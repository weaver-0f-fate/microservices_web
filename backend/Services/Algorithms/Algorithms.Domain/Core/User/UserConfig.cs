namespace Algorithms.Domain.Core.User;

public class UserConfig
{
    public int RolesCacheMinutes { get; set; } = 5;
    public int RolesRequestTimeoutSeconds { get; set; } = 15;
}