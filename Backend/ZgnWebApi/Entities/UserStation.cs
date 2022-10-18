using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class UserStation : IEntity<UserStation>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StationId { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<UserStation, ZgnAgvManagerContext>
        {
        }
        public IDataResult<List<UserStation>> GetAll(Expression<Func<UserStation, bool>>? filter = null)
        {
            return new SuccessDataResult<List<UserStation>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<UserStation?> Get(Expression<Func<UserStation, bool>> filter)
        {
            return new SuccessDataResult<UserStation?>(_repository.Get(filter), "Get filter data");
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
        public async Task<IDataResult<List<UserStation>>> GetAllAsync(Expression<Func<UserStation, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<UserStation>>(result, "Listed all data");
        }
        public async Task<IDataResult<UserStation?>> GetAsync(Expression<Func<UserStation, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<UserStation?>(result, "Get filter data");
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
