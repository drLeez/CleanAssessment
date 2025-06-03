using CleanAssessment.DB;
using CleanAssessment.DB.Models;
using CleanAssessment.Shared.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        //IRepositoryAsync<TEntity, TId> Repository<TEntity, TId>() where TEntity : Entity<TId>;
        Task<IList<T>> StoredProcOrFunction<T>(string query, params object[] parameters) where T : class;
        Task<int> Commit(CancellationToken cancellationToken);
        Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
        Task Rollback();

        IRepositoryAsync<Customer, int> CustomerRepository { get; set; }
        IRepositoryAsync<PaymentMethod, int> PaymentMethodRepository { get; set; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _dbContext;
        private readonly Type IRepositoryAsyncType = typeof(IRepositoryAsync<,>)
            , UnitOfWorkType = typeof(UnitOfWork);

        private bool _disposed;
        private Hashtable? _repositories;

        public IRepositoryAsync<Customer, int> CustomerRepository { get; set; }
        public IRepositoryAsync<PaymentMethod, int> PaymentMethodRepository { get; set; }

        public UnitOfWork(MyDbContext dbContext)
        {
            _dbContext = dbContext;

            var repoMethod = UnitOfWorkType.GetMethod(nameof(Repository), BindingFlags.NonPublic | BindingFlags.Instance);
            var props = UnitOfWorkType.GetProperties();

            foreach (var prop in props)
            {
                var tEntityType = prop.PropertyType.GetGenericArguments()[0];
                var tIdType = prop.PropertyType.GetGenericArguments()[1];
                var call = repoMethod.MakeGenericMethod(tEntityType, tIdType);
                var value = call.Invoke(this, null);
                prop.SetValue(this, value, null);
            }
        }

        private IRepositoryAsync<TEntity, TId> Repository<TEntity, TId>() where TEntity : Entity<TId>
        {
            _repositories ??= new Hashtable();

            var typeName = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(typeName))
            {
                var repositoryType = typeof(RepositoryAsync<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TId)), _dbContext);

                _repositories.Add(typeName, repositoryInstance);
            }

            return (IRepositoryAsync<TEntity, TId>)_repositories[typeName];
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

        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public async Task<IList<T>> StoredProcOrFunction<T>(string query, params object[] parameters) where T : class
        {
            return await _dbContext.Set<T>().FromSqlRaw(query, parameters).AsNoTracking().ToListAsync();
        }

        Task<IList<T>> IUnitOfWork.StoredProcOrFunction<T>(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        Task<int> IUnitOfWork.Commit(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<int> IUnitOfWork.CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            throw new NotImplementedException();
        }

        Task IUnitOfWork.Rollback()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
