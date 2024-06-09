using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Algorithms.API.Authentication;

public class UserTokenAuthenticationFailedHandler(ILogger<UserTokenAuthenticationFailedHandler> logger) : IJwtBearerEventHandler<AuthenticationFailedContext>
{
    public Task Handle(AuthenticationFailedContext context)
    {
        logger.LogError(context.Exception, nameof(UserTokenAuthenticationFailedHandler));
        return Task.CompletedTask;
    }
}