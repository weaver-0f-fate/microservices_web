using System.Security.Claims;

namespace Algorithms.Domain.Core.User;

public static class UserClaimTypes
{
    public const string Username = "preferred_username";
    public const string Name = ClaimTypes.Name;
    public const string Role = ClaimTypes.Role;
    public const string GivenName = ClaimTypes.GivenName;
    public const string Surname = ClaimTypes.Surname;
}