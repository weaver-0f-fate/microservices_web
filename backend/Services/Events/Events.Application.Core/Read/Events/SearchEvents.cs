using Events.Application.Core.Contracts;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Specifications;

namespace Events.Application.Core.Read.Events;

public interface ISearchEvents : IQuery<string?, IEnumerable<Event>> { }

public class SearchEvents(IRepository<Event> repository) : ISearchEvents
{
    public async Task<IEnumerable<Event>> ExecuteAsync(string? searchString, CancellationToken cancellationToken)
    {
        var spec = new SearchEventsSpecification(searchString);
        var result = await repository.ListAsync(spec, cancellationToken);

        return result;
    }
}