using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class AuthorityOperationClaim : IEntity<AuthorityOperationClaim>
    {
        public int Id { get; set; }
        public int AuthorityId { get; set; }
        public int OperationClaimId { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<AuthorityOperationClaim, ZgnAgvManagerContext>
        {
        }
        public IDataResult<List<AuthorityOperationClaim>> GetAll(Expression<Func<AuthorityOperationClaim, bool>>? filter = null)
        {
            return new SuccessDataResult<List<AuthorityOperationClaim>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<AuthorityOperationClaim?> Get(Expression<Func<AuthorityOperationClaim, bool>> filter)
        {
            return new SuccessDataResult<AuthorityOperationClaim?>(_repository.Get(filter), "Get filter data");
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
        public async Task<IDataResult<List<AuthorityOperationClaim>>> GetAllAsync(Expression<Func<AuthorityOperationClaim, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<AuthorityOperationClaim>>(result, "Listed all data");
        }
        public async Task<IDataResult<AuthorityOperationClaim?>> GetAsync(Expression<Func<AuthorityOperationClaim, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<AuthorityOperationClaim?>(result, "Get filter data");
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
