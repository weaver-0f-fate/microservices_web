using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notification.API.Filters;
using Notification.API.Middleware;
using Notification.Domain.Aggregates.Base;
using Notification.Infrastructure;
using Notification.Infrastructure.Repositories;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseLamar(ConfigureContainer);

    var services = builder.Services;

    services.AddControllers(options =>
    {
        options.Filters.Add<NotFoundExceptionFilterAttribute>();
    })
        .AddNewtonsoftJson();


    services.AddHttpClient("EventsHttpClient", httpClient =>
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var baseUrl = environment == "Development"
            ? "http://localhost:7203"
            : "http://eventsapi";
        httpClient.BaseAddress = new Uri(baseUrl);
    });

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    await services.AddDbContext(connectionString!);

    services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssemblyContaining(typeof(Notification.Application.Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Notification.Infrastructure.Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    });
    ConfigureAuth(services, builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ExceptionMiddleware>();

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
    services.For<IRepository<Notification.Domain.Aggregates.Notification>>().Use<NotificationsRepository<Notification.Domain.Aggregates.Notification>>();

    services.Scan(scan => { scan.TheCallingAssembly(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Notification.Application.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Notification.Infrastructure.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Notification.Domain.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Notification.Application.Core.Assembly>(); scan.WithDefaultConventions(); });

    services.Scan(scan => { scan.AssemblyContainingType<Assembly>(); scan.WithDefaultConventions(); });
}
static void ConfigureAuth(IServiceCollection services, IConfiguration conf)
{
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    var identityUrl = conf.GetValue<string>("IdentityUrl");

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.Authority = identityUrl;
        options.RequireHttpsMetadata = false;
        options.Audience = "notifications";
    });
}