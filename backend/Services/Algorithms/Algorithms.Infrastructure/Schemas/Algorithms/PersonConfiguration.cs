using Algorithms.Domain.Aggregates;
using Algorithms.Infrastructure.Configuration;
using Algorithms.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Algorithms.Infrastructure.Schemas.Algorithms;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    private readonly Dialect _dialect;

    public PersonConfiguration(Dialect dialect)
    {
        _dialect = dialect;
    }

    public void Configure(EntityTypeBuilder<Person> builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        var map = new EntityMapper<Person>(builder, _dialect);
        map.ToTable();
    }
}

