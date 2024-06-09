using Algorithms.Domain.Core.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Algorithms.API.Authentication;

public class UserTokenValidatedContextHandler(UserContext userContext) : IJwtBearerEventHandler<TokenValidatedContext>
{
    public Task Handle(TokenValidatedContext context)
    {
        if (!context.Principal.HasClaim(c => c.Type == UserClaimTypes.Username))
            context.Fail($"The claim '{UserClaimTypes.Username}' is not present in the token.");
        if (!context.Principal.HasClaim(c => c.Type == UserClaimTypes.Name))
            context.Fail($"The claim '{UserClaimTypes.Name}' is not present in the token.");

        userContext.SetUserId(context.Principal.FindFirst(c => c.Type == UserClaimTypes.Username).Value);
        //userContext.SetName(context.Principal.FindFirst(c => c.Type == UserClaimTypes.Name).Value);
        userContext.SetGivenName(context.Principal.FindFirst(c => c.Type == UserClaimTypes.GivenName).Value);
        userContext.AccessToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        userContext.SetSurname(context.Principal.FindFirst(c => c.Type == UserClaimTypes.Surname).Value);

        return Task.CompletedTask;
    }
}
