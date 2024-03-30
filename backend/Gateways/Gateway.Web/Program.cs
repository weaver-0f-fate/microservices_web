using Gateway.Web;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseLamar(ConfigureContainer)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName.ToLower()}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        });


    // Add & configure services
    var services = builder.Services;

    ConfigureCors(services);
    //ConfigureAuth(services, builder.Configuration);
    services.AddOcelot();


    var app = builder.Build();
    // Configure the HTTP request pipeline.

    if (app.Environment.IsDevelopment())
    {
        //ignore
    }

    app.UseCors("AllowSpecificOrigin");
    app.UseOcelot().Wait();

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

return;

static void ConfigureContainer(ServiceRegistry services)
{
    services.Scan(scan => { scan.AssemblyContainingType<Assembly>(); scan.WithDefaultConventions(); });
}

static void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", builder =>
        {
            builder
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

static void ConfigureAuth(IServiceCollection services, IConfiguration config)
{
    var identityUrl = config.GetValue<string>("IdentityUrl");
    const string authenticationProviderKey = "IdentityApiKey";
    var audiences = new[] { "events", "notifications", "subscriptions" };


    services.AddAuthentication()
        .AddJwtBearer(authenticationProviderKey, x =>
        {
            x.Authority = identityUrl;
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidAudiences = audiences
            };
        });
}