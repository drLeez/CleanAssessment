using CleanAssessment.Shared.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public interface IRepositoryAsync<TEntity, TId> where TEntity : Entity<TId>
    {
        IQueryable<TEntity> Entities { get; }
        Task<TEntity> GetByIdAsync(TId id);
        Task<TEntity> GetByCodeAsync(string code);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetPagedResponseAsync(int page, int pageSize);

        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(List<TEntity> entities);
        Task DeleteRangeAsync(IQueryable<TEntity> entities);
        Task DeleteRangeAsync(ICollection<TEntity> entities);
    }
}
