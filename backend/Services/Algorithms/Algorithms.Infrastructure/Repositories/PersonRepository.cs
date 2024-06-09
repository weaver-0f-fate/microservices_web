using Algorithms.Domain.Aggregates;
using Algorithms.Domain.Aggregates.Repositories;
using Algorithms.Domain.Core.Interfaces;
using Algorithms.Infrastructure.Context;
namespace Algorithms.Infrastructure.Repositories;

public class PersonRepository(WriteDatabaseContext context) : IPersonRepository
{
    public IUnitOfWork UnitOfWork => context;

    public void Add(Person person)
    {
        context.Add(person);
    }

    public Person? Get(Guid uuid)
    {
        return context.Person.FirstOrDefault(person => person.Uuid == uuid);
    }
}

