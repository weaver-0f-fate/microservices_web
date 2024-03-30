using Events.Application.Core.Read.Events;
using MediatR;

namespace Events.Application.Requests.Events;

public class GetEventHandler(IGetEvent getEvent) : IRequestHandler<GetEventRequest, GetEventResponse>
{
    public async Task<GetEventResponse> Handle(GetEventRequest request, CancellationToken cancellationToken)
    {
        var result = await getEvent.ExecuteAsync(request.Uuid, cancellationToken);

        return new GetEventResponse
        {
            Event = new EventDto
            {
                Uuid = result.Id,
                Category = result.Category,
                Title = result.Title,
                ImageUrl = result.ImageUrl,
                Description = result.Description,
                Place = result.Place,
                Date = result.GetDateWithTime(),
                AdditionalInfo = result.AdditionalInfo,
                Recurrency = result.Recurrency
            }
        };
    }
}

public class GetEventRequest : IRequest<GetEventResponse>
{
    public Guid Uuid { get; set; }
}

public class GetEventResponse
{
    public EventDto Event { get; set; }
}

public struct EventDto
{
    public Guid Uuid { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public string Place { get; set; }
    public DateTime Date { get; set; }
    public string AdditionalInfo { get; set; }
    public string? Recurrency { get; set; }
}