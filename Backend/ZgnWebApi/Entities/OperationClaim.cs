using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class OperationClaim : BaseTimeStamp, IPageableEntity<OperationClaim>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<OperationClaim, ZgnAgvManagerContext>
        {
        }
        public IDataResult<IPaginationResult<List<OperationClaim>>> GetAllWithPagination(IPageableFilter<OperationClaim>? pageableFilter = null)
        {
            return new SuccessDataResult<IPaginationResult<List<OperationClaim>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<OperationClaim>> GetAll(Expression<Func<OperationClaim, bool>>? filter = null)
        {
            return new SuccessDataResult<List<OperationClaim>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<OperationClaim?> Get(Expression<Func<OperationClaim, bool>> filter)
        {
            return new SuccessDataResult<OperationClaim?>(_repository.Get(filter), "Get filter data");
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
        public ISingleResult CheckAndAddByName(string name)
        {
            if (_repository.Get(e => e.Name == name) == null)
            {
                this.Name = name;
                Add();
            }
            return new SuccessResult("Checked and added");
        }
        public ISingleResult SoftDelete()
        {
            _repository.SoftDelete(this);
            return new SuccessResult("Soft Deleted");
        }
        public async Task<IDataResult<IPaginationResult<List<OperationClaim>>>> GetAllWithPaginationAsync(IPageableFilter<OperationClaim>? pageableFilter = null)
        {
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<OperationClaim>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<OperationClaim>>> GetAllAsync(Expression<Func<OperationClaim, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<OperationClaim>>(result, "Listed all data");
        }
        public async Task<IDataResult<OperationClaim?>> GetAsync(Expression<Func<OperationClaim, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<OperationClaim?>(result, "Get filter data");
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

        public IDataResult<IPaginationResult<List<OperationClaim>>> GetAllSelectedByAuthorityId(int authorityId, IPageableFilter<OperationClaim> pageableFilter)
        {
            var claimIds = new AuthorityOperationClaim().GetAll(e => e.AuthorityId == authorityId).Data.Select(e => e.OperationClaimId);
            return new SuccessDataResult<IPaginationResult<List<OperationClaim>>>(_repository.GetAllWithPagination(new PageableFilter<OperationClaim>(pageableFilter.Filter.And(e => claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by in authority data");

        }

        public IDataResult<IPaginationResult<List<OperationClaim>>> GetAllUnSelectedByAuthorityId(int authorityId, IPageableFilter<OperationClaim> pageableFilter)
        {
            var claimIds = new AuthorityOperationClaim().GetAll(e => e.AuthorityId == authorityId).Data.Select(e => e.OperationClaimId);
            return new SuccessDataResult<IPaginationResult<List<OperationClaim>>>(_repository.GetAllWithPagination(new PageableFilter<OperationClaim>(pageableFilter.Filter.And(e => !claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by not in authority data");
        }
        public ISingleResult SaveSelectedByAuthorityId(int authorityId, List<int> claimIds)
        {
            var dbClaims = new AuthorityOperationClaim().GetAll(e => e.AuthorityId == authorityId).Data;
            claimIds.Where(c => !dbClaims.Select(e => e.OperationClaimId).Contains(c)).ToList().ForEach(id => new AuthorityOperationClaim() { AuthorityId = authorityId, OperationClaimId = id }.Add());
            dbClaims.Where(c => !claimIds.Contains(c.OperationClaimId)).ToList().ForEach(claim => claim.Delete());
            return new SuccessResult("List saved");
        }
        public IDataResult<IPaginationResult<List<OperationClaim>>> GetAllSelectedByUserId(int userId, IPageableFilter<OperationClaim> pageableFilter)
        {
            //var claimIds = new UserOperationClaim().GetAll(e => e.UserId == userId).Data.Select(e => e.OperationClaimId);
            var claimIds=new User().Get(u => u.Id == userId).Data?.GetClaims().Select(e => e.Id);
            return new SuccessDataResult<IPaginationResult<List<OperationClaim>>>(_repository.GetAllWithPagination(new PageableFilter<OperationClaim>(pageableFilter.Filter.And(e => claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by in user data");

        }

        public IDataResult<IPaginationResult<List<OperationClaim>>> GetAllUnSelectedByUserId(int userId, IPageableFilter<OperationClaim> pageableFilter)
        {
            //var claimIds = new UserOperationClaim().GetAll(e => e.UserId == userId).Data.Select(e => e.OperationClaimId);
            var claimIds = new User().Get(u => u.Id == userId).Data?.GetClaims().Select(e => e.Id);
            return new SuccessDataResult<IPaginationResult<List<OperationClaim>>>(_repository.GetAllWithPagination(new PageableFilter<OperationClaim>(pageableFilter.Filter.And(e => !claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by not in user data");
        }
        public ISingleResult SaveSelectedByUserId(int userId, List<int> claimIds)
        {
            var dbClaims = new UserOperationClaim().GetAll(e => e.UserId == userId).Data;
            claimIds.Where(c => !dbClaims.Select(e => e.OperationClaimId).Contains(c)).ToList().ForEach(id => new UserOperationClaim() { UserId = userId, OperationClaimId = id }.Add());
            dbClaims.Where(c => !claimIds.Contains(c.OperationClaimId)).ToList().ForEach(claim => claim.Delete());
            return new SuccessResult("List saved");
        }

    }
}
