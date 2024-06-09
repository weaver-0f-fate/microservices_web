using Algorithms.Domain.Aggregates.AlgorithmAggregate;
using Algorithms.Domain.Providers;
using Algorithms.Infrastructure.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Algorithms.Infrastructure.Context;

public sealed class WriteDatabaseContext : TransactionalDatabaseContext
{

    internal DbSet<Algorithm> Algorithms => Set<Algorithm>();

    public WriteDatabaseContext(
        IOptions<PostgreSqlConfigration> postgreSqlConfiguration,
        IOptions<EventsConfiguration> eventsConfiguration,
        IMediator mediator,
        IUserIdProvider userIdProvider,
        ILogger<WriteDatabaseContext> logger,
        ILoggerFactory loggerFactory)
        : base(postgreSqlConfiguration, eventsConfiguration, mediator, userIdProvider, logger, loggerFactory)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
            throw new ArgumentNullException(nameof(modelBuilder));

        AutoConfigurator.MapSchemas(modelBuilder, Dialect.Postgres, typeof(Assembly));
    }
}


