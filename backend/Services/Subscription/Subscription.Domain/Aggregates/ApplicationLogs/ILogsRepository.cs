namespace Subscription.Domain.Aggregates.ApplicationLogs;

public interface ILogsRepository
{
    public Task CreateLogAsync(LogRecord log);
}