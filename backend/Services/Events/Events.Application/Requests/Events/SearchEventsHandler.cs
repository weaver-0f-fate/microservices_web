using Events.Application.Core.Read.Events;
using MediatR;

namespace Events.Application.Requests.Events;

public class SearchEventsHandler : IRequestHandler<SearchEventsRequest, SearchEventsResponse>
{
    private readonly ISearchEvents _searchEvents;

    public SearchEventsHandler(ISearchEvents searchEvents)
    {
        _searchEvents = searchEvents;
    }

    public async Task<SearchEventsResponse> Handle(SearchEventsRequest request, CancellationToken cancellationToken)
    {
        var result = await _searchEvents.ExecuteAsync(request.SearchString, cancellationToken);

        return new SearchEventsResponse
        {
            Events = result.Select(@event => new SearchEventDto
            {
                Uuid = @event.Id,
                Title = @event.Title,
                Description = @event.Description,
                Place = @event.Place
            })
        };
    }

}

public class SearchEventsRequest : IRequest<SearchEventsResponse>
{
    public string? SearchString { get; set; }
}

public class SearchEventsResponse
{
    public IEnumerable<SearchEventDto> Events { get; set; } = new List<SearchEventDto>();
}

public struct SearchEventDto
{
    public Guid Uuid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Place { get; set; }
}