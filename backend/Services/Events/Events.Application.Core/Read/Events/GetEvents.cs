using Events.Application.Core.Contracts;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Specifications;

namespace Events.Application.Core.Read.Events;

public interface IGetEvents : IQuery<GetEventsInput, IEnumerable<Event>> { }

public struct GetEventsInput
{
    public string? Category { get; set; }
    public string? Place { get; set; }
    public TimeSpan? StartDate { get; set; }
}

public class GetEvents : IGetEvents
{
    private readonly IRepository<Event> _repository;

    public GetEvents(IRepository<Event> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Event>> ExecuteAsync(GetEventsInput input, CancellationToken cancellationToken)
    {
        var spec = new EventSpecification(input.Category, input.Place, input.StartDate);
        var result = await _repository.ListAsync(spec, cancellationToken);
        return result;
    }
}