using Ardalis.Specification;
using Events.Domain.Aggregates;
using System.Linq.Expressions;

namespace Events.Domain.Specifications;

public sealed class EventSpecification : Specification<Event>
{
    public EventSpecification(string? category, string? place, TimeSpan? eventStartingDate)
    {
        Criteria = e =>
            (string.IsNullOrEmpty(category) || e.Category.Contains(category)) &&
            (string.IsNullOrEmpty(place) || e.Place.Contains(place)) &&
            (!eventStartingDate.HasValue || e.UtcTime >= eventStartingDate);

        Query.Where(Criteria);
    }

    private Expression<Func<Event, bool>> Criteria { get; }
}
