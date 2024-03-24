using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscription.Domain.Aggregates.ApplicationLogs;
using Subscription.Infrastructure.MongoConfiguration;

namespace Subscription.Infrastructure.Repositories;

public class LogsRepository : ILogsRepository
{
    private readonly IMongoCollection<LogRecord> _logsCollection;
    public LogsRepository(IOptions<AuditLogDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _logsCollection = mongoDatabase.GetCollection<LogRecord>(
            bookStoreDatabaseSettings.Value.LogsCollectionName);
    }

    public async Task CreateLogAsync(LogRecord log)
    {
        await _logsCollection.InsertOneAsync(log);
    }
}