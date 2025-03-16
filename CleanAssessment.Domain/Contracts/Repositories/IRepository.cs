using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Contracts.Repositories
{
    public interface IRepositoryAsync<T, in I> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<T> GetByIdAsync(I id);
        Task<T> GetByCodeAsync(string code);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetPagedResponseAsync(int page, int pageSize);

        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entities);
        Task DeleteRangeAsync(IQueryable<T> entities);
        Task DeleteRangeAsync(ICollection<T> entities);
    }
}
