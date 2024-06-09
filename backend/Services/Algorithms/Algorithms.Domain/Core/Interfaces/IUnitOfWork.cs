﻿namespace Algorithms.Domain.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

