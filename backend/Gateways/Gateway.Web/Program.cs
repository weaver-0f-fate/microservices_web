using Gateway.Web;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
    ConfigureAuth(services);
    services.AddOcelot(builder.Configuration);


    var app = builder.Build();
    // Configure the HTTP request pipeline.

    if (app.Environment.IsDevelopment())
    {
        //ignore
    }

    app.UseRouting();
    app.UseCors("AllowSpecificOrigin");
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

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
                    //var handleTokenValidatedContext = context.HttpContext.RequestServices.GetService<IJwtBearerEventHandler<TokenValidatedContext>>();
                    //if (handleTokenValidatedContext != null)
                    //return handleTokenValidatedContext.Handle(context);

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