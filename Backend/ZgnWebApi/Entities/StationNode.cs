using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class StationNode : BaseTimeStamp, IEntity<StationNode>
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public string? NodeId { get; set; }
        public int ColPosition { get; set; } = -1;
        public int RowPosition { get; set; } = -1;
        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<StationNode, ZgnAgvManagerContext>
        {
        }
        public IDataResult<List<StationNode>> GetAll(Expression<Func<StationNode, bool>>? filter = null)
        {
            return new SuccessDataResult<List<StationNode>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<StationNode?> Get(Expression<Func<StationNode, bool>> filter)
        {
            return new SuccessDataResult<StationNode?>(_repository.Get(filter), "Get filter data");
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
        public ISingleResult CheckAndAdd()
        {
            if (_repository.Get(e => e.NodeId == NodeId && e.StationId == StationId) == null)
            {
                Add();
            }
            return new SuccessResult("Checked and added");
        }
        public async Task<IDataResult<List<StationNode>>> GetAllAsync(Expression<Func<StationNode, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<StationNode>>(result, "Listed all data");
        }
        public async Task<IDataResult<StationNode?>> GetAsync(Expression<Func<StationNode, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<StationNode?>(result, "Get filter data");
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
