using Events.Application.Core.UseCases.Events;
using MediatR;

namespace Events.Application.Commands.Events;

public class DeleteEventHandler(IDeleteEvent deleteEvent) : IRequestHandler<DeleteEventRequest>
{
    public async Task Handle(DeleteEventRequest request, CancellationToken cancellationToken)
    {
        await deleteEvent.InvokeAsync(request.Uuid, cancellationToken);
    }
}

public class DeleteEventRequest : IRequest
{
    public Guid Uuid { get; set; }
}