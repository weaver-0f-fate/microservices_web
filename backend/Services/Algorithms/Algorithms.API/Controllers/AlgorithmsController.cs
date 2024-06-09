using Algorithms.Domain.Aggregates;
using Algorithms.Domain.Aggregates.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Algorithms.API.Controllers;

[Route("api/algorithms")]
[ApiController]
public class AlgorithmsController(IPersonRepository personRepository) : ControllerBase
{

    public async Task<IActionResult> GetAsync(CancellationToken token)
    {
        var person = new Person(Guid.NewGuid(), "John Doe", "123 Main St", "Apt 1", "Anytown", "US", "john.doe@gmail.com");

        personRepository.Add(person);

        await personRepository.UnitOfWork.SaveChangesAsync(token);

        var result = new
        {
            personUuid = person.Uuid
        };
        return Ok(result);
    }
}