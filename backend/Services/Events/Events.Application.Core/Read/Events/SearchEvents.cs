using Events.Application.Core.Contracts;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Specifications;

namespace Events.Application.Core.Read.Events;

public interface ISearchEvents : IQuery<string?, IEnumerable<Event>> { }

public class SearchEvents : ISearchEvents
{
    private readonly IRepository<Event> _repository;

    public SearchEvents(IRepository<Event> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Event>> ExecuteAsync(string? searchString, CancellationToken cancellationToken)
    {
        var spec = new SearchEventsSpecification(searchString);
        var result = await _repository.ListAsync(spec, cancellationToken);

        return result;
    }
}