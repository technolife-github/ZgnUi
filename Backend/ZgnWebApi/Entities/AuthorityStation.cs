using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class AuthorityStation : IEntity<AuthorityStation>
    {
        public int Id { get; set; }
        public int AuthorityId { get; set; }
        public int StationId { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<AuthorityStation, ZgnAgvManagerContext>
        {
        }
        public IDataResult<List<AuthorityStation>> GetAll(Expression<Func<AuthorityStation, bool>>? filter = null)
        {
            return new SuccessDataResult<List<AuthorityStation>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<AuthorityStation?> Get(Expression<Func<AuthorityStation, bool>> filter)
        {
            return new SuccessDataResult<AuthorityStation?>(_repository.Get(filter), "Get filter data");
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
        public async Task<IDataResult<List<AuthorityStation>>> GetAllAsync(Expression<Func<AuthorityStation, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<AuthorityStation>>(result, "Listed all data");
        }
        public async Task<IDataResult<AuthorityStation?>> GetAsync(Expression<Func<AuthorityStation, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<AuthorityStation?>(result, "Get filter data");
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

    }
}
