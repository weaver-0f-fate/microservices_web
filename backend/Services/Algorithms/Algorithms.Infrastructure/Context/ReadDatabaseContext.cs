using Algorithms.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Algorithms.Infrastructure.Context;

public class ReadDatabaseContext
{
    private readonly WriteDatabaseContext _context;

    public ReadDatabaseContext(WriteDatabaseContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> Set<TEntity>() where TEntity : Entity
    {
        return _context.Set<TEntity>().AsNoTracking();
    }
}