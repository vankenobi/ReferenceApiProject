using System;
using Reference.Api.Data;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;

namespace Reference.Api.Repositories.Implementations
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataContext _context;
        public readonly ILogger _logger;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(DataContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger("logs");
            _repositories = new Dictionary<Type, object>();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            // Rollback changes if needed
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new GenericRepository<TEntity>(_context, _logger);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}

