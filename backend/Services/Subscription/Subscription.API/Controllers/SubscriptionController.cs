using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subscription.Application.Commands;

namespace Subscription.API.Controllers;

[Route("api/subscription")]
[ApiController]
[Authorize]
public class SubscriptionController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    [Authorize(Roles = "User, Instructor, Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> SubscribeUserAsync([FromBody] SubscribeUserRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        var resourceUri = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/subscription/{result.SubscriptionUuid}");
        return Created(resourceUri, result);
    }
}