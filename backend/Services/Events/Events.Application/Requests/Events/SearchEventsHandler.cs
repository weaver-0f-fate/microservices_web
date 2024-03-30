using Events.Application.Core.Read.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Events.Application.Requests.Events;

public class SearchEventsHandler(ISearchEvents searchEvents) : IRequestHandler<SearchEventsRequest, SearchEventsResponse>
{
    public async Task<SearchEventsResponse> Handle(SearchEventsRequest request, CancellationToken cancellationToken)
    {
        var result = await searchEvents.ExecuteAsync(request.SearchString, cancellationToken);

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
    public IEnumerable<SearchEventDto> Events { get; set; } = default!;
}

public struct SearchEventDto
{
    public Guid Uuid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Place { get; set; }
}