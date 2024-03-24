namespace Events.Infrastructure.MongoConfiguration;

public class AuditLogDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string LogsCollectionName { get; set; } = null!;
}