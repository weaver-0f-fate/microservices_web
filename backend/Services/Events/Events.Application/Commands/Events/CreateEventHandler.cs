using Events.Application.Core.UseCases.Events;
using Events.Application.Requests.Events;
using MediatR;

namespace Events.Application.Commands.Events;

public class CreateEventHandler : IRequestHandler<CreateEventRequest, CreateEventResponse>
{
    private readonly ICreateEvent _createEvent;

    public CreateEventHandler(ICreateEvent createEvent)
    {
        _createEvent = createEvent;
    }

    public async Task<CreateEventResponse> Handle(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var result = await _createEvent.InvokeAsync(new CreateEventInput
        {
            Category = request.Category,
            Title = request.Title,
            ImageUrl = request.ImageUrl,
            Place = request.Place,
            Description = request.Description,
            AdditionalInfo = request.AdditionalInfo,
            Date = request.Date
        }, cancellationToken);

        return new CreateEventResponse
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
                AdditionalInfo = result.AdditionalInfo
            }
        };
    }
}

public struct CreateEventRequest : IRequest<CreateEventResponse>
{
    public string Category { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string Place { get; set; }
    public string Description { get; set; }
    public string AdditionalInfo { get; set; }
    public DateTimeOffset Date { get; set; }
}

public struct CreateEventResponse
{
    public EventDto Event { get; set; }
}