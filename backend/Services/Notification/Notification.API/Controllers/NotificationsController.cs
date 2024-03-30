using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands;
using Notification.Application.Requests;

namespace Notification.API.Controllers;

[Route("api/notifications")]
[ApiController]
[Authorize]
public class NotificationsController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet("{notificationId}")]
    [Authorize(Roles = "User, Instructor, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await Mediator.Send(new GetNotificationRequest { NotificationId = notificationId }, cancellationToken);
        return Ok(notification);
    }

    [HttpPost]
    [Authorize(Roles = "User, Instructor, Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNotificationAsync(CreateEventNotificationRequest request, CancellationToken cancellationToken)
    {
        var notificationId = await Mediator.Send(request, cancellationToken);
        var resourceUri = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/notifications/{notificationId}");
        return Created(resourceUri, notificationId);
    }
}