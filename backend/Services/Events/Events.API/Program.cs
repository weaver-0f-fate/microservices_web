using Events.API.Filters;
using Events.API.Middleware;
using Events.Domain.Aggregates.Base;
using Events.Infrastructure;
using Events.Infrastructure.MongoConfiguration;
using Events.Infrastructure.Repositories;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Event = Events.Domain.Aggregates.Event;

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
            options.Filters.Add<NotFoundExceptionFilterAttribute>();
        })
        .AddNewtonsoftJson();

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    await services.AddDbContext(connectionString);

    services.Configure<AuditLogDatabaseSettings>(builder.Configuration.GetSection("AuditLoggingDatabase"));

    services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssemblyContaining(typeof(Events.Application.Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Assembly));
        cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    });
    ConfigureSwagger(services);
    ConfigureCors(services);
    ConfigureAuth(services, builder.Configuration);


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ExceptionMiddleware>();
    //app.UseCors();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Events API V1");
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
            Title = "Events API",
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
    services.For<IRepository<Event>>().Use<EventsRepository<Event>>();

    services.Scan(scan => { scan.TheCallingAssembly(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Events.Application.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Events.Infrastructure.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Events.Domain.Assembly>(); scan.WithDefaultConventions(); });
    services.Scan(scan => { scan.AssemblyContainingType<Events.Application.Core.Assembly>(); scan.WithDefaultConventions(); });

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
        options.Audience = "events";
    });
}