using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.Extensions.Options;
using Algorithms.Infrastructure.Configuration;
using Algorithms.Infrastructure.Exceptions;
using Algorithms.Domain.Core;
using Algorithms.Domain.Core.Interfaces;
using Algorithms.Domain.Providers;

namespace Algorithms.Infrastructure.Context;

public class TransactionalDatabaseContext : DbContext, IUnitOfWork
{
    private readonly string _connectionString;

    private readonly IMediator _mediator;

    /// <summary>
    /// Tracks the count of domain events published during the lifetime of this context.
    /// </summary>
    public int CountOfEventsPublished { get; set; }

    private readonly int _publishedEventsLimit;

    private IDbContextTransaction CurrentTransaction { get; set; }

    public bool HasActiveTransaction => CurrentTransaction != null;
    public Guid? CurrentTransactionId => CurrentTransaction?.TransactionId;

    private readonly IUserIdProvider _userIdProvider;
    private readonly ILogger? _logger;
    private readonly ILoggerFactory? _loggerFactory;

    public TransactionalDatabaseContext(
        IOptions<PostgreSqlConfigration> databaseConfiguration,
        IOptions<EventsConfiguration> eventsConfiguration,
        IMediator mediator,
        IUserIdProvider userIdProvider,
        ILogger? logger = null,
        ILoggerFactory? loggerFactory = null
    )
    {
        _connectionString = databaseConfiguration.Value.GetConnectionString();
        _publishedEventsLimit = eventsConfiguration.Value.PublishingLimit;
        _mediator = mediator;
        _userIdProvider = userIdProvider;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_connectionString)
            .UseSnakeCaseNamingConvention();
#if DEBUG
        optionsBuilder
            .UseLoggerFactory(_loggerFactory)
            .EnableSensitiveDataLogging();
#endif
    }

    private async Task HandleDomainEventsAsync(CancellationToken cancellationToken)
    {
        var entitiesWithEventsPublished = ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = entitiesWithEventsPublished
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        CountOfEventsPublished += domainEvents.Count;

        if (CountOfEventsPublished > _publishedEventsLimit)
            throw new DomainEventsOverloadException(CountOfEventsPublished, _publishedEventsLimit);

        foreach (var entity in entitiesWithEventsPublished)
            entity.Entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        foreach (var ev in domainEvents)
            _logger.LogInformation("Published domain event {Event}", ev.GetType().Name);
    }

    public new int SaveChanges()
    {
        return SaveChanges(true);
    }

    public new int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();
    }

    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(true, cancellationToken);
    }

    public new async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    HandleCreated(entry);
                    break;
                case EntityState.Deleted:
                    HandleDeleted(entry);
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                case EntityState.Modified:
                    HandleAudit(entry);
                    break;
            }
        }

        var saveChanges = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await HandleDomainEventsAsync(cancellationToken);

        return saveChanges;
    }

    private void HandleDeleted(EntityEntry entityEntry)
    {
        entityEntry.State = EntityState.Unchanged;

        HandleAudit(entityEntry);

        var entity = entityEntry.Entity;
        if (entity is not IDeleteable deleteable)
            return;

        deleteable.IsActive = false;
    }

    private void HandleCreated(EntityEntry entityEntry)
    {
        var entity = entityEntry.Entity;

        HandleAudit(entityEntry);

        if (entity is not IDeleteable deleteable)
            return;

        deleteable.IsActive = true;
    }

    private void HandleAudit(EntityEntry entityEntry)
    {
        var entity = entityEntry.Entity;
        var userId = _userIdProvider.Get();

        if (entity is not IAudit auditEntity || userId == null)
            return;

        auditEntity.AuditAuthor = userId;
        auditEntity.AuditDtime = DateTime.UtcNow;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (CurrentTransaction != null)
            return null;

        CurrentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return CurrentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));
        if (transaction != CurrentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (CurrentTransaction != null)
            {
                await CurrentTransaction.DisposeAsync();
                CurrentTransaction = null;
            }
        }
    }

    private async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (CurrentTransaction is not null)
                await CurrentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }
    }
}