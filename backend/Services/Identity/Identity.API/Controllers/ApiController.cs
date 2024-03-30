using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
public class ApiController(IMediator mediator) : ControllerBase
{
    public readonly IMediator Mediator = mediator;
}