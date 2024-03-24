using Events.Application.Core.UseCases.Events;
using MediatR;

namespace Events.Application.Commands.Events;

public class DeleteEventHandler : IRequestHandler<DeleteEventRequest>
{
    private readonly IDeleteEvent _deleteEvent;

    public DeleteEventHandler(IDeleteEvent deleteEvent)
    {
        _deleteEvent = deleteEvent;
    }

    public async Task Handle(DeleteEventRequest request, CancellationToken cancellationToken)
    {
        await _deleteEvent.InvokeAsync(request.Uuid, cancellationToken);
    }
}

public class DeleteEventRequest : IRequest
{
    public Guid Uuid { get; set; }
}