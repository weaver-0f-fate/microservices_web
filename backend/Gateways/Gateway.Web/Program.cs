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

    ConfigureAuth(services, builder.Configuration);
    services.AddOcelot();


    var app = builder.Build();
    // Configure the HTTP request pipeline.

    if (app.Environment.IsDevelopment())
    {
        //ignore
    }

    app.UseRouting();
    app.UseOcelot().Wait();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

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
    services.Scan(scan => { scan.AssemblyContainingType<Assembly>(); scan.WithDefaultConventions(); });
}

static void ConfigureAuth(IServiceCollection services, IConfiguration config)
{
    var identityUrl = config.GetValue<string>("IdentityUrl");
    var authenticationProviderKey = "IdentityApiKey";

    services.AddAuthentication()
        .AddJwtBearer(authenticationProviderKey, x =>
        {
            x.Authority = identityUrl;
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidAudiences = new[] { "events", "notifications", "subscriptions" }
            };
        });
}