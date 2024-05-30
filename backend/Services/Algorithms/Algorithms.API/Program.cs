using System.Security.Claims;
using Algorithms.API;
using Algorithms.API.Middleware;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseLamar(ConfigureContainer);
    var services = builder.Services;


    services.AddControllers(options =>
    {
        //options.Filters.Add<NotFoundExceptionFilterAttribute>();
    }).AddNewtonsoftJson();

    //services.AddAutoMapper(typeof(EventProfile));

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    //await services.AddDbContext(connectionString);

    //services.Configure<AuditLogDatabaseSettings>(builder.Configuration.GetSection("AuditLoggingDatabase"));

    services.AddMediatR(cfg => {
        //cfg.RegisterServicesFromAssemblyContaining(typeof(Events.Application.Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    });

    ConfigureAuth(services);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

static void ConfigureContainer(ServiceRegistry services)
{
    //services.For<IRepository<Event>>().Use<EventsRepository<Event>>();

    services.Scan(scan => { scan.TheCallingAssembly(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Algorithms.Application.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Algorithms.Infrastructure.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Algorithms.Domain.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Algorithms.Application.Core.Assembly>(); scan.WithDefaultConventions(); });

    services.Scan(scan => { scan.AssemblyContainingType<Assembly>(); scan.WithDefaultConventions(); });
}

static void ConfigureAuth(IServiceCollection services)
{
    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(cfg =>
        {
            cfg.Authority = "http://localhost:8036/auth/realms/mweb_personnel";
            cfg.MetadataAddress = "http://localhost:8036/auth/realms/mweb_personnel/.well-known/openid-configuration";
            cfg.RequireHttpsMetadata = false;

            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:8036/auth/realms/mweb_personnel",
                ValidateAudience = true,
                ValidAudiences = new[] { "frontend", "mobile", "swagger", "events", "notifications", "subscriptions", "account" },
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = new TimeSpan(0, 0, 30)
            };

            cfg.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    //var handleTokenValidatedContext = context.HttpContext.RequestServices.GetService<IJwtBearerEventHandler<TokenValidatedContext>>();
                    //if (handleTokenValidatedContext != null)
                    //return handleTokenValidatedContext.Handle(context);

                    return Task.CompletedTask;
                },
                OnForbidden = context =>
                {
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    //var handleAuthenticationFailedContext = context.HttpContext.RequestServices.GetService<IJwtBearerEventHandler<AuthenticationFailedContext>>();
                    //if (handleAuthenticationFailedContext != null)
                    //    return handleAuthenticationFailedContext.Handle(context);

                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    // todo: with signalR we are sending access token as query param!
                    // fixme: if we dont filter out, we log it, if we log it someone can steal it!!!
                    // todo: ill stress one more time, DONT FORGET IT !
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    return Task.CompletedTask;
                }
            };
        });
}