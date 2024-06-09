namespace Algorithms.Infrastructure.Configuration;

public class PostgreSqlConfigration
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
    public int? MaxConnectionPoolSize { get; set; } = 50;
    public string Username { get; set; }
    public string Password { get; set; }

    public string GetConnectionString()
    {
        return $"host={Host};{(Port != null ? $"port={Port};" : "")}database={Database};username={Username};password={Password};Maximum pool size={MaxConnectionPoolSize}";
    }
}

