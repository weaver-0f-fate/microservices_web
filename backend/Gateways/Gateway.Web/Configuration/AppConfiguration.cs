namespace Gateway.Web.Configuration;

public class AppConfiguration
{
    public string Environment { get; set; } = default!;
    public string EnvironmentDisplayName { get; set; } = default!;
    public string Version { get; set; } = default!;
    public KeycloakConfiguration Keycloak { get; set; } = default!;
}

public class KeycloakConfiguration
{
    public Uri AuthUrl { get; set; } = default!;
    public string Realm { get; set; } = default!;
    public string ClientId { get; set; } = default!;
}