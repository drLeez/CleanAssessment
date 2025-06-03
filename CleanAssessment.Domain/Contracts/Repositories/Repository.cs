using CleanAssessment.DB;
using CleanAssessment.Shared.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public class RepositoryAsync<TEntity, TId> : IRepositoryAsync<TEntity, TId> where TEntity : Entity<TId>
    {
        private readonly MyDbContext _dbContext;
        public RepositoryAsync(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<TEntity> Entities => _dbContext.Set<TEntity>();

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        public Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(List<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IQueryable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(ICollection<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public Task<TEntity> GetByIdAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetByCodeAsync(string code)
        {
            return await _dbContext.Set<TEntity>().FindAsync(code);
        }

        public async Task<List<TEntity>> GetPagedResponseAsync(int page, int pageSize)
        {
            var raw = _dbContext.Set<TEntity>().Skip((page - 1) * pageSize).Take(pageSize);
            return await raw.AsNoTracking().ToListAsync();
        }

        public Task UpdateAsync(TEntity entity) // must be an object type with an Id
        {
            Type type = entity.GetType();
            PropertyInfo prop = type.GetProperty("Id");
            var find = _dbContext.Set<TEntity>().Find(prop.GetValue(entity));
            _dbContext.Entry(find).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
    }
}
