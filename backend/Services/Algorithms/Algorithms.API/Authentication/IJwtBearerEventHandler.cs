using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace Algorithms.API.Authentication;

public interface IJwtBearerEventHandler<T> where T : ResultContext<JwtBearerOptions>
{
    Task Handle(T context);
}

