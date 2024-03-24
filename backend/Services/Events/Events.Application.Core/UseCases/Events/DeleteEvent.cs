using Events.Application.Core.Contracts;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Exceptions;
using Events.Domain.Specifications;
using System.Transactions;
using Events.Domain.Aggregates.ApplicationLogs;
using Serilog;

namespace Events.Application.Core.UseCases.Events;

public interface IDeleteEvent : IUseCaseAsync<Guid> { }

public class DeleteEvent : IDeleteEvent
{
    private readonly IRepository<Event> _repository;
    private readonly ILogsRepository _logsRepository;

    public DeleteEvent(IRepository<Event> repository, ILogsRepository logsRepository)
    {
        _repository = repository;
        _logsRepository = logsRepository;
    }

    public async Task InvokeAsync(Guid eventUuid, CancellationToken cancellationToken)
    {
        try
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var spec = new EventByUuidSpecification(eventUuid);
            var @event = await _repository.SingleOrDefaultAsync(spec, cancellationToken) 
                         ?? throw new NotFoundException($"Event with uuid {eventUuid} not found.");
            await _repository.DeleteAsync(@event, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            await _logsRepository.CreateLogAsync(new LogRecord
            {
                AuditTime = DateTime.UtcNow,
                Action = $"Deleted event with id: {eventUuid}"
            });

            transactionScope.Complete();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while deleting event with id: {EventUuid}", eventUuid);
            throw;
        }
    }
}