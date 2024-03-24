using System.Globalization;
using Events.Application.Commands.Events;
using Events.Application.Requests.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventsController : ApiController
{
    public EventsController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [Authorize(Roles = "Guest, User, Instructor, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? category, [FromQuery] string? place, [FromQuery] string? time, CancellationToken cancellationToken)
    {
        var timeFilter = TimeSpan.TryParse(time, CultureInfo.CurrentCulture, out var t);
        var result = await Mediator.Send(new GetEventsRequest
        {
            Category = category,
            Place = place,
            StartDate = timeFilter ? t : null
        }, cancellationToken);
        return Ok(result.Events);
    }

    [HttpGet("search")]
    [Authorize(Roles = "Guest, User, Instructor, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchEvents([FromQuery] string? searchString, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchEventsRequest
        {
            SearchString = searchString
        }, cancellationToken);
        return Ok(result.Events);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        var resourceUri = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/events/{result.Event.Uuid}");
        return Created(resourceUri, result.Event);
    }
}