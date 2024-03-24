using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    public readonly IMediator Mediator;

    public ApiController(IMediator mediator)
    {
        Mediator = mediator;
    }
}