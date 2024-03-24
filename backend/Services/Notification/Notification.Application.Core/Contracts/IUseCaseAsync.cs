namespace Notification.Application.Core.Contracts;

public interface IUseCaseAsync<in TInput, TOutput>
{
    Task<TOutput> InvokeAsync(TInput input, CancellationToken cancellationToken);
}

public interface IUseCaseAsync<in TInput>
{
    public Task InvokeAsync(TInput input, CancellationToken cancellationToken);
}