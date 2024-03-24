namespace Notification.Application.Core.Contracts;

public interface IQuery<in TQuery, TResult>
{
    Task<TResult> ExecuteAsync(TQuery eventUuid, CancellationToken cancellationToken);
}

public interface IQuery<TResult>
{
    Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
}