using System.Text.Json;
using Flurl;
using Gateway.Web.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Controllers;

[Route("api/configuration")]
[ApiController]
public class ConfigurationController(IConfiguration configuration) : ApiController
{
    [HttpGet]
    public IActionResult Get()
    {
        var appConfiguration = 
            configuration.GetSection("Application")
                .Get<AppConfiguration>() ?? throw new ArgumentNullException("App configuration is not initialized");

        appConfiguration.Keycloak.AuthUrl = appConfiguration.Keycloak.AuthUrl.AppendPathSegments("realms", appConfiguration.Keycloak.Realm).ToUri();

        return Ok(JsonSerializer.Serialize(appConfiguration));
    }
}