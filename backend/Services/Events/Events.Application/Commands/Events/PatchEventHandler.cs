using Events.Application.Core.DTOs;
using Events.Application.Core.UseCases.Events;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Events.Application.Commands.Events;

public class PatchEventHandler : IRequestHandler<PatchEventRequest>
{
    private readonly IPatchEvent _patchEvent;

    public PatchEventHandler(IPatchEvent patchEvent)
    {
        _patchEvent = patchEvent;
    }

    public async Task Handle(PatchEventRequest request, CancellationToken cancellationToken)
    {
        await _patchEvent.InvokeAsync(new PatchEventInput
        {
            EventUuid = request.EventUuid,
            PatchDoc = request.PatchDoc
        }, cancellationToken);
    }
}

public class PatchEventRequest : IRequest
{
    public Guid EventUuid { get; set; }
    public JsonPatchDocument<UpdateEventDto> PatchDoc { get; set; } = default!;
}