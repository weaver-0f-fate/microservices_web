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

public class DeleteEvent(IRepository<Event> repository, ILogsRepository logsRepository) : IDeleteEvent
{
    public async Task InvokeAsync(Guid eventUuid, CancellationToken cancellationToken)
    {
        try
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var spec = new EventByUuidSpecification(eventUuid);
            var @event = await repository.SingleOrDefaultAsync(spec, cancellationToken) 
                         ?? throw new NotFoundException($"Event with uuid {eventUuid} not found.");
            await repository.DeleteAsync(@event, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            await logsRepository.CreateLogAsync(new LogRecord
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