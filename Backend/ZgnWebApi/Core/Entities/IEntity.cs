using System.Linq.Expressions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;

namespace ZgnWebApi.Core.Entities
{
    public interface IColumn
    {
    }
    public interface IEntity : IColumn
    {
        public int Id { get; set; }
    }
    public interface IEntityAsync<T> : IEntity
    {
        Task<IDataResult<List<T>>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<IDataResult<T?>> GetAsync(Expression<Func<T, bool>> filter);
        Task<ISingleResult> AddAsync();
        Task<ISingleResult> UpdateAsync();
        Task<ISingleResult> DeleteAsync();
    }
    public interface IEntity<T> : IEntityAsync<T>
    {
        IDataResult<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        IDataResult<T?> Get(Expression<Func<T, bool>> filter);
        ISingleResult Add();
        ISingleResult Update();
        ISingleResult Delete();
    }
    public interface IPageableEntity<T> : IEntity<T>
    {
        public DateTime CreatedAt { get; set; }
        public int CreatedUser { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedUser { get; set; }
        IDataResult<IPaginationResult<List<T>>> GetAllWithPagination(IPageableFilter<T>? pageableFilter = null);
        ISingleResult SoftDelete();
        Task<IDataResult<IPaginationResult<List<T>>>> GetAllWithPaginationAsync(IPageableFilter<T>? pageableFilter = null);
        Task<ISingleResult> SoftDeleteAsync();
    }
    public interface IDto : IColumn
    {
    }
    public abstract class BaseTimeStamp
    {
        public DateTime CreatedAt { get; set; }
        public int CreatedUser { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedUser { get; set; }
        public BaseTimeStamp()
        {
            CreatedAt = DateTime.Now;
        }
    }

}
