using CleanAssessment.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public interface IUnitOfWork<I> : IDisposable
    {
        IRepositoryAsync<T, I> Repository<T>() where T : class;
        Task<IList<T>> StoredProcOrFunction<T>(string query, params object[] parameters) where T : class;
        Task<int> Commit(CancellationToken cancellationToken);
        Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
        Task Rollback();
    }
    public class UnitOfWork<I> : IUnitOfWork<I>
    {
        private readonly MyDbContext _dbContext;
        private bool _disposed;
        private Hashtable _repositories;

        public UnitOfWork(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            foreach (var cacheKey in cacheKeys)
            {
                // not really implementing cache for this example ...
            }
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }

        public IRepositoryAsync<T, I> Repository<T>() where T : class
        {
            _repositories ??= new Hashtable();

            var typeName = typeof(T).Name;
            if (!_repositories.ContainsKey(typeName))
            {
                var repositoryType = typeof(RepositoryAsync<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T), typeof(I)), _dbContext);

                _repositories.Add(typeName, repositoryInstance);
            }

            return (IRepositoryAsync<T, I>)_repositories[typeName];
        }

        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public async Task<IList<T>> StoredProcOrFunction<T>(string query, params object[] parameters) where T : class
        {
            return await _dbContext.Set<T>().FromSqlRaw(query, parameters).AsNoTracking().ToListAsync();
        }
    }
}
