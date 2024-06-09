using Algorithms.Domain.Aggregates.AlgorithmAggregate;
using Algorithms.Domain.Aggregates.Repositories;
using Algorithms.Domain.Core.Interfaces;
using Algorithms.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Algorithms.Infrastructure.Repositories;

public class AlgorithmsRepository(WriteDatabaseContext context) : IAlgorithmsRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<Algorithm> AddAsync(Algorithm algorithm, CancellationToken token)
    {
        var newAlgorithm = await context.AddAsync(algorithm, token);
        return newAlgorithm.Entity;
    }

    public async Task<Algorithm?> GetAsync(Guid uuid, CancellationToken token)
    {
        return await context.Algorithms.FirstOrDefaultAsync(algorithm => algorithm.Uuid == uuid, token);
    }
}

