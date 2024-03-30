using System.Transactions;
using AutoMapper;
using Events.Application.Core.Contracts;
using Events.Application.Core.DTOs;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.ApplicationLogs;
using Events.Domain.Aggregates.Base;
using Events.Domain.Exceptions;
using Events.Domain.Specifications;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;

namespace Events.Application.Core.UseCases.Events;

public interface IPatchEvent : IUseCaseAsync<PatchEventInput> { }

public struct PatchEventInput
{
    public Guid EventUuid { get; set; }
    public JsonPatchDocument<UpdateEventDto> PatchDoc { get; set; }
}

public class PatchEvent(IRepository<Event> repository, IMapper mapper, ILogsRepository logsRepository) : IPatchEvent
{
    public async Task InvokeAsync(PatchEventInput input, CancellationToken cancellationToken)
    {
        try
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var spec = new EventByUuidSpecification(input.EventUuid);
            var @event = await repository.SingleOrDefaultAsync(spec, cancellationToken)
                         ?? throw new NotFoundException($"Event with uuid {input.EventUuid} not found.");

            var updateEventDto = new UpdateEventDto(@event);
            input.PatchDoc.ApplyTo(updateEventDto);

            var updatedEvent = mapper.Map<Event>(updateEventDto);
            @event.Update(updatedEvent);

            await repository.UpdateAsync(@event, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            await logsRepository.CreateLogAsync(new LogRecord
            {
                AuditTime = DateTime.UtcNow,
                Action = $"Updated event with id: {input.EventUuid}"
            });

            transactionScope.Complete();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while patching event with id: {EventUuid}", input.EventUuid);
            throw;
        }
    }
}