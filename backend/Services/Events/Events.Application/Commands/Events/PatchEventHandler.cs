using Events.Application.Core.DTOs;
using Events.Application.Core.UseCases.Events;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Events.Application.Commands.Events;

public class PatchEventHandler(IPatchEvent patchEvent) : IRequestHandler<PatchEventRequest>
{
    public async Task Handle(PatchEventRequest request, CancellationToken cancellationToken)
    {
        await patchEvent.InvokeAsync(new PatchEventInput
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