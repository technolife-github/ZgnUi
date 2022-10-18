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
    public class Authority : BaseTimeStamp, IPageableEntity<Authority>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<Authority, ZgnAgvManagerContext>
        {
        }
        public IDataResult<IPaginationResult<List<Authority>>> GetAllWithPagination(IPageableFilter<Authority>? pageableFilter = null)
        {
            return new SuccessDataResult<IPaginationResult<List<Authority>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<Authority>> GetAll(Expression<Func<Authority, bool>>? filter = null)
        {
            return new SuccessDataResult<List<Authority>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<Authority?> Get(Expression<Func<Authority, bool>> filter)
        {
            return new SuccessDataResult<Authority?>(_repository.Get(filter), "Get filter data");
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
        public async Task<IDataResult<IPaginationResult<List<Authority>>>> GetAllWithPaginationAsync(IPageableFilter<Authority>? pageableFilter = null)
        {
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<Authority>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<Authority>>> GetAllAsync(Expression<Func<Authority, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<Authority>>(result, "Listed all data");
        }
        public async Task<IDataResult<Authority?>> GetAsync(Expression<Func<Authority, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<Authority?>(result, "Get filter data");
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

        public IDataResult<IPaginationResult<List<Authority>>> GetAllSelectedByUserId(int userId, IPageableFilter<Authority> pageableFilter)
        {
            var claimIds = new UserAuthority().GetAll(e => e.UserId == userId).Data.Select(e => e.AuthorityId);
            return new SuccessDataResult<IPaginationResult<List<Authority>>>(_repository.GetAllWithPagination(new PageableFilter<Authority>(pageableFilter.Filter.And(e => claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by in user data");
        }
        public IDataResult<IPaginationResult<List<Authority>>> GetAllUnSelectedByUserId(int userId, IPageableFilter<Authority> pageableFilter)
        {
            var claimIds = new UserAuthority().GetAll(e => e.UserId == userId).Data.Select(e => e.AuthorityId);
            return new SuccessDataResult<IPaginationResult<List<Authority>>>(_repository.GetAllWithPagination(new PageableFilter<Authority>(pageableFilter.Filter.And(e => !claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by not in user data");
        }
        public ISingleResult SaveSelectedByUserId(int userId, List<int> claimIds)
        {
            var dbClaims = new UserAuthority().GetAll(e => e.UserId == userId).Data;
            claimIds.Where(c => !dbClaims.Select(e => e.AuthorityId).Contains(c)).ToList().ForEach(id => new UserAuthority() { UserId = userId, AuthorityId = id }.Add());
            dbClaims.Where(c => !claimIds.Contains(c.AuthorityId)).ToList().ForEach(claim => claim.Delete());
            return new SuccessResult("List saved");
        }
    }
}
