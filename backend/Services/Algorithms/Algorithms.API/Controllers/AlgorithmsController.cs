using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Algorithms.API.Controllers;

[Route("api/algorithms")]
[ApiController]
public class AlgorithmsController : ControllerBase
{

    public async Task<IActionResult> Get()
    {
        var result = new
        {
            msg = "Hello From Algorithms Controller"
        };
        return Ok(result);
    }
}