using System.Linq.Expressions;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;

namespace ZgnWebApi.Core.DataAccess
{
    public interface IBaseRepository<T> where T : class, IEntity, new()
    {
        IPaginationResult<List<T>> GetAllWithPagination(IPageableFilter<T>? pageableFilter);
        List<T> GetAll(Expression<Func<T, bool>>? filter = null);
        T? Get(Expression<Func<T, bool>> filter);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool SoftDelete(T entity);
    }

}
