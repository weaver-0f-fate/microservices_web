using Ardalis.Specification;
using Events.Domain.Aggregates;
using System.Linq.Expressions;

namespace Events.Domain.Specifications;

public sealed class SearchEventsSpecification : Specification<Event>
{
    public SearchEventsSpecification(string? searchString)
    {
        Criteria = e =>
            string.IsNullOrEmpty(searchString) || 
            e.Title.Contains(searchString) || 
            e.Description.Contains(searchString) ||
            e.Place.Contains(searchString);

        Query.Where(Criteria);
    }

    private Expression<Func<Event, bool>> Criteria { get; }
}