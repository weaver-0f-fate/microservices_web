using Events.Application.Commands.Events;
using Events.Application.Core.DTOs;
using Events.Application.Requests.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace Events.API.Controllers;

[Route("api/events/{uuid}")]
[ApiController]
//[Authorize]
public class EventDetailsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEventRequest
        {
            Uuid = uuid
        }, cancellationToken);

        return Ok(result.Event);
    }

    [HttpDelete]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteEventRequest
        {
            Uuid = uuid
        }, cancellationToken);

        return NoContent();
    }

    [HttpPatch]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Patch(
        [FromRoute] Guid uuid,
        [FromBody] JsonPatchDocument<UpdateEventDto>? patchDoc,
        CancellationToken cancellationToken)
    {
        if (patchDoc is null)
            return BadRequest();

        await mediator.Send(new PatchEventRequest
        {
            EventUuid = uuid,
            PatchDoc = patchDoc
        }, cancellationToken);

        return NoContent();
    }
}