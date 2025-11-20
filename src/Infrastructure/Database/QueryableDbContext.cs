using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database;

public abstract class QueryableDbContext<TDbContext>
    : IDisposable
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    private bool _disposedValue;

    protected QueryableDbContext(IDbContextFactory<TDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);

        _dbContext = dbContextFactory.CreateDbContext();

        ChangeTracker changeTracker = _dbContext.ChangeTracker;
        changeTracker.AutoDetectChangesEnabled = false;
        changeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected IQueryable<TEntity> AsNoTracking<TEntity>()
        where TEntity : class
    {
        return _dbContext.Set<TEntity>().AsNoTracking();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposedValue = true;
        }
    }
}
