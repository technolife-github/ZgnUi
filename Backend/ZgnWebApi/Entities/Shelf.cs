using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class Shelf : BaseTimeStamp, IPageableEntity<Shelf>
    {
        public int Id { get; set; }
        public string ShelfTypeId { get; set; }
        public string? Name { get; set; }
        public string? NodeId { get; set; }
        public string? TransportId { get; set; }
        public bool IsStorage { get; set; } = false;

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<Shelf, ZgnAgvManagerContext>
        {
        }
        public IDataResult<IPaginationResult<List<Shelf>>> GetAllWithPagination(IPageableFilter<Shelf>? pageableFilter = null)
        {
            return new SuccessDataResult<IPaginationResult<List<Shelf>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<Shelf>> GetAll(Expression<Func<Shelf, bool>>? filter = null)
        {
            return new SuccessDataResult<List<Shelf>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<Shelf?> Get(Expression<Func<Shelf, bool>> filter)
        {
            return new SuccessDataResult<Shelf?>(_repository.Get(filter), "Get filter data");
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
        public async Task<IDataResult<IPaginationResult<List<Shelf>>>> GetAllWithPaginationAsync(IPageableFilter<Shelf>? pageableFilter = null)
        {
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<Shelf>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<Shelf>>> GetAllAsync(Expression<Func<Shelf, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<Shelf>>(result, "Listed all data");
        }
        public async Task<IDataResult<Shelf?>> GetAsync(Expression<Func<Shelf, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<Shelf?>(result, "Get filter data");
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