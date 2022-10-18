using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.IoC;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class Log : BaseTimeStamp, IPageableEntity<Log>
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string? Type { get; set; }//Error,Warning,Info,Debug
        public string? LogText { get; set; }
        public Log()
        {
        }
        public Log(int transactionId, LogType type, string logText)
        {
            TransactionId = transactionId;
            UserId = ServiceTool.GetUserId();
            Type = type.Value;
            LogText = logText;
        }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<Log, ZgnAgvManagerContext>
        {
        }
        public IDataResult<IPaginationResult<List<Log>>> GetAllWithPagination(IPageableFilter<Log>? pageableFilter = null)
        {
            return new SuccessDataResult<IPaginationResult<List<Log>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<IPaginationResult<List<Log>>> GetAllByTransactionIdUi(int transactionId, IPageableFilter<Log>? pageableFilter = null)
        {
            pageableFilter.Filter = pageableFilter.Filter.And(e => e.TransactionId == transactionId);
            return new SuccessDataResult<IPaginationResult<List<Log>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<Log>> GetAll(Expression<Func<Log, bool>>? filter = null)
        {
            return new SuccessDataResult<List<Log>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<Log?> Get(Expression<Func<Log, bool>> filter)
        {
            return new SuccessDataResult<Log?>(_repository.Get(filter), "Get filter data");
        }
        public ISingleResult Add()
        {
            UserId = ServiceTool.GetUserId();
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
        public async Task<IDataResult<IPaginationResult<List<Log>>>> GetAllWithPaginationAsync(IPageableFilter<Log>? pageableFilter = null)
        {
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<Log>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<Log>>> GetAllAsync(Expression<Func<Log, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<Log>>(result, "Listed all data");
        }
        public async Task<IDataResult<Log?>> GetAsync(Expression<Func<Log, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<Log?>(result, "Get filter data");
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
