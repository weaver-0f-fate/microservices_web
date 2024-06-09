using Algorithms.Domain.Aggregates.AlgorithmAggregate;
using Algorithms.Infrastructure.Configuration;
using Algorithms.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Algorithms.Infrastructure.Schemas.Algorithms;

public class AlgorithmConfiguration : IEntityTypeConfiguration<Algorithm>
{
    private readonly Dialect _dialect;

    public AlgorithmConfiguration(Dialect dialect)
    {
        _dialect = dialect;
    }

    public void Configure(EntityTypeBuilder<Algorithm> builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        var map = new EntityMapper<Algorithm>(builder, _dialect);
        map.ToTable();
    }
}