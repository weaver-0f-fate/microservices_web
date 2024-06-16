using Algorithms.Domain.Aggregates.AlgorithmAggregate;
using Algorithms.Domain.Core.Interfaces;

namespace Algorithms.Domain.Aggregates.Repositories;
public interface IAlgorithmsRepository : IRepository
{
    public Task<Algorithm> AddAsync(Algorithm algorithm, CancellationToken token);
    public Task<Algorithm?> GetAsync(Guid uuid, CancellationToken token);
    public Task<IEnumerable<Algorithm>> GetAsync(CancellationToken token);
}

