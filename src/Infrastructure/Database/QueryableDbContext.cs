using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database;

public abstract class QueryableDbContext<TContext> : IDisposable
   where TContext : DbContext
{
    private readonly TContext _context;

    private bool _disposedValue;

    protected QueryableDbContext(DbContextOptions<TContext> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _context = (TContext)Activator.CreateInstance(typeof(TContext), options)!;

        ChangeTracker changeTracker = _context.ChangeTracker;
        changeTracker.AutoDetectChangesEnabled = false;
        changeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected IQueryable<TEntity> AsNoTracking<TEntity>()
        where TEntity : class => _context.Set<TEntity>().AsNoTracking();

    public IQueryable<TEntity> AsNoTrackingWithIdentityResolution<TEntity>()
        where TEntity : class => _context.Set<TEntity>().AsNoTrackingWithIdentityResolution();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _context?.Dispose();
            }

            _disposedValue = true;
        }
    }

    ~QueryableDbContext()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
