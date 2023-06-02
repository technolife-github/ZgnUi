using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class ShelfType : BaseTimeStamp, IPageableEntity<ShelfType>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<ShelfType, ZgnAgvManagerContext>
        {
        }
        public IDataResult<IPaginationResult<List<ShelfType>>> GetAllWithPagination(IPageableFilter<ShelfType>? pageableFilter = null)
        {
            return new SuccessDataResult<IPaginationResult<List<ShelfType>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<ShelfType>> GetAll(Expression<Func<ShelfType, bool>>? filter = null)
        {
            return new SuccessDataResult<List<ShelfType>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<ShelfType?> Get(Expression<Func<ShelfType, bool>> filter)
        {
            return new SuccessDataResult<ShelfType?>(_repository.Get(filter), "Get filter data");
        }
        public ISingleResult Add()
        {
            _repository.Add(this);
            return new SuccessResult("Added");
        }
        public ISingleResult Update()
        {
            _repository.Update(this);
            return new SuccessResult("Updated");
        }
        public ISingleResult Delete()
        {
            _repository.Delete(this);
            return new SuccessResult("Deleted");
        }
        public ISingleResult SoftDelete()
        {
            _repository.SoftDelete(this);
            return new SuccessResult("Soft Deleted");
        }
        public async Task<IDataResult<IPaginationResult<List<ShelfType>>>> GetAllWithPaginationAsync(IPageableFilter<ShelfType>? pageableFilter = null)
        {
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<ShelfType>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<ShelfType>>> GetAllAsync(Expression<Func<ShelfType, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<ShelfType>>(result, "Listed all data");
        }
        public async Task<IDataResult<ShelfType?>> GetAsync(Expression<Func<ShelfType, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<ShelfType?>(result, "Get filter data");
        }
        public async Task<ISingleResult> AddAsync()
        {
            await _repository.AddAsync(this);
            return new SuccessResult("Added");
        }
        public async Task<ISingleResult> UpdateAsync()
        {
            await _repository.UpdateAsync(this);
            return new SuccessResult("Updated");
        }
        public async Task<ISingleResult> DeleteAsync()
        {
            await _repository.DeleteAsync(this);
            return new SuccessResult("Deleted");
        }
        public async Task<ISingleResult> SoftDeleteAsync()
        {
            await _repository.SoftDeleteAsync(this);
            return new SuccessResult("Soft Deleted");
        }

    }
}
