using Events.Application.Core.Read.Events;
using MediatR;

namespace Events.Application.Requests.Events;

public class GetEventsHandler(IGetEvents getEvents) : IRequestHandler<GetEventsRequest, GetEventsResponse>
{
    public async Task<GetEventsResponse> Handle(GetEventsRequest request, CancellationToken cancellationToken)
    {
        var results = await getEvents.ExecuteAsync(new GetEventsInput
        {
            Category = request.Category,
            Place = request.Place,
            StartDate = request.StartDate
        }, cancellationToken);

        return new GetEventsResponse
        {
            Events = results.Select(@event => new EventBaseInfo
            {
                Uuid = @event.Id,
                Title = @event.Title,
                Date = @event.GetDateWithTime(),
                Rrule = @event.Recurrency,
                Category = @event.Category,
                Place = @event.Place
            })
        };
    }
}

public class GetEventsRequest : IRequest<GetEventsResponse>
{
    public string? Category { get; set; }
    public string? Place { get; set; }
    public TimeSpan? StartDate { get; set; }
}

public class GetEventsResponse
{
    public IEnumerable<EventBaseInfo> Events { get; set; } = default!;
}

public class EventBaseInfo
{
    public Guid Uuid { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Rrule { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
}