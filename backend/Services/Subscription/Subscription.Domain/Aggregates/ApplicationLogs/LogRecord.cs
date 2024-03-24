using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Subscription.Domain.Aggregates.ApplicationLogs;

public class LogRecord
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime AuditTime { get; set; }
    public string? Action { get; set; }

}