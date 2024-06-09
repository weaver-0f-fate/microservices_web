using Algorithms.Domain.Core.Interfaces;

namespace Algorithms.Domain.Aggregates.Repositories;

public interface IPersonRepository : IRepository
{
    public void Add(Person person);
    public Person? Get(Guid uuid);
}

