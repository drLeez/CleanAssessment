using CleanAssessment.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public class RepositoryAsync<T, I> : IRepositoryAsync<T, I> where T : class
    {
        private readonly MyDbContext _dbContext;
        public RepositoryAsync(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IQueryable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByCodeAsync(string code)
        {
            return await _dbContext.Set<T>().FindAsync(code);
        }

        public async Task<T> GetByIdAsync(I id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetPagedResponseAsync(int page, int pageSize)
        {
            var raw = _dbContext.Set<T>().Skip((page - 1) * pageSize).Take(pageSize);
            return await raw.AsNoTracking().ToListAsync();
        }

        public Task UpdateAsync(T entity) // must be an object type with an Id
        {
            Type type = entity.GetType();
            PropertyInfo prop = type.GetProperty("Id");
            var find = _dbContext.Set<T>().Find(prop.GetValue(entity));
            _dbContext.Entry(find).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
    }
}
