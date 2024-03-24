using Ardalis.Specification;
using Events.Domain.Aggregates;

namespace Events.Domain.Specifications;

public sealed class EventByUuidSpecification : SingleResultSpecification<Event>
{
    public EventByUuidSpecification(Guid uuid)
    {
        Query.Where(@event => @event.Id == uuid);
    }
}