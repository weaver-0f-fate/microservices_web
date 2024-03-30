using Events.Application.Core.Contracts;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Serilog;
using System.Transactions;
using Events.Domain.Aggregates.ApplicationLogs;

namespace Events.Application.Core.UseCases.Events;

public interface ICreateEvent : IUseCaseAsync<CreateEventInput, Event> { }

public struct CreateEventInput
{
    public string Category { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string Place { get; set; }
    public string Description { get; set; }
    public string AdditionalInfo { get; set; }
    public DateTimeOffset Date { get; set; }
}


public class CreateEvent(IRepository<Event> repository, ILogsRepository logsRepository) : ICreateEvent
{
    public async Task<Event> InvokeAsync(CreateEventInput input, CancellationToken cancellationToken)
    {
        try
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var @event = new Event
            {
                Id = Guid.NewGuid(),
                Category = input.Category,
                Title = input.Title,
                ImageUrl = input.ImageUrl,
                Place = input.Place,
                Description = input.Description,
                AdditionalInfo = input.AdditionalInfo
            };
            @event.UpdateDateTime(input.Date);

            var createdEvent = await repository.AddAsync(@event, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            await logsRepository.CreateLogAsync(new LogRecord
            {
                AuditTime = DateTime.UtcNow,
                Action = $"Created event with id: {createdEvent.Id}"
            });

            transactionScope.Complete();
            return createdEvent;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while creating event");
            throw;
        }
    }
}