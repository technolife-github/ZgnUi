using System.Linq.Expressions;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;

namespace ZgnWebApi.Core.DataAccess
{
    public interface IBaseAsyncRepository<T> where T : class, IEntity, new()
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> SoftDeleteAsync(T entity);
        Task<IPaginationResult<List<T>>> GetAllWithPaginationAsync(IPageableFilter<T>? pageableFilter);
    }

}
