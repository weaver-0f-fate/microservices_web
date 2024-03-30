using Events.Application.Core.Contracts;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Exceptions;
using Events.Domain.Specifications;

namespace Events.Application.Core.Read.Events;

public interface IGetEvent : IQuery<Guid, Event> { }

public class GetEvent(IRepository<Event> repository) : IGetEvent
{
    public async Task<Event> ExecuteAsync(Guid eventUuid, CancellationToken cancellationToken)
    {
        var spec = new EventByUuidSpecification(eventUuid);

        var @event = await repository.SingleOrDefaultAsync(spec, cancellationToken);
        return @event ?? throw new NotFoundException($"Event with uuid {eventUuid} not found.");
    }
}