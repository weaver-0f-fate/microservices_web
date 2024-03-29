using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Subscription.API.Filters;
using Subscription.API.Middleware;
using Subscription.Infrastructure.Repositories;
using Subscription.Infrastructure;
using Subscription.Domain.Aggregates;
using Subscription.Domain.Aggregates.Base;
using Subscription.Infrastructure.MongoConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    }).AddNewtonsoftJson();

    services.AddHttpClient("NotificationHttpClient", httpClient =>
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var baseUrl = environment == "Development"
            ? "http://localhost:7205"
            : "http://notificationsapi";
        httpClient.BaseAddress = new Uri(baseUrl);
    });

    ConfigureCors(services);
    ConfigureSwagger(services);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    await services.AddDbContext(connectionString!);

    services.Configure<AuditLogDatabaseSettings>(builder.Configuration.GetSection("AuditLoggingDatabase"));

    services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssemblyContaining(typeof(Subscription.Application.Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Subscription.Infrastructure.Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    });
    ConfigureAuth(services, builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseCors();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subscriptions API V1");
    });

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

static void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Subscriptions API",
            Version = "v1",
        });
    });
}
static void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}
static void ConfigureContainer(ServiceRegistry services)
{
    services.For<IRepository<Subscription.Domain.Aggregates.Subscription>>().Use<SubscriptionsRepository<Subscription.Domain.Aggregates.Subscription>>();
    services.For<IRepository<User>>().Use<UsersRepository<User>>();

    services.Scan(scanner => { scanner.AssemblyContainingType<Program>(); scanner.WithDefaultConventions(); });
    services.Scan(scan => { scan.TheCallingAssembly(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Subscription.Application.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Subscription.Infrastructure.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Subscription.Domain.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Subscription.Application.Core.Assembly>(); scan.WithDefaultConventions(); });

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
        options.Audience = "subscriptions";
    });
}