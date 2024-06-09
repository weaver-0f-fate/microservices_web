using Algorithms.Domain.Aggregates;
using Algorithms.Domain.Aggregates.AlgorithmAggregate;
using Algorithms.Domain.Aggregates.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Algorithms.API.Controllers;

[Route("api/algorithms")]
[ApiController]
public class AlgorithmsController(IAlgorithmsRepository algorithmRepository) : ControllerBase
{

    public async Task<IActionResult> GetAsync(CancellationToken token)
    {
        var algorithm = new Algorithm("MyNewAlgorithm");

        var newAlgorithm = await algorithmRepository.AddAsync(algorithm, token);

        await algorithmRepository.UnitOfWork.SaveChangesAsync(token);

        var result = new
        {
            algorithmUuid = algorithm.Uuid
        };
        return Ok(result);
    }
}