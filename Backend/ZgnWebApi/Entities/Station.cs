using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Entities.DTOs;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class Station : BaseTimeStamp, IPageableEntity<Station>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ColumnLen { get; set; }
        public int RowLen { get; set; }
        [NotMapped]
        public OptionDataModel? TypeView { get; set; }
        public string? Type { get; set; }//Insertion,Pickup,Charger
        [NotMapped]
        public virtual List<OptionDataModel>? Nodes { get; set; }
        [NotMapped]
        public virtual List<StationNode>? StationNodes { get; set; }
        [NotMapped]
        public virtual List<OptionDataModel>? GroupCodes { get; set; }
        [NotMapped]
        public virtual List<StationGroupCode>? StationGroupCodes { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<Station, ZgnAgvManagerContext>
        {
            public override List<Station> GetAll(Expression<Func<Station, bool>>? filter)
            {
                using (var context = new ZgnAgvManagerContext())
                {
                    var result = from station in context.Stations
                                 select new Station()
                                 {
                                     Id = station.Id,
                                     RowLen = station.RowLen,
                                     ColumnLen = station.ColumnLen,
                                     Name = station.Name,
                                     Type = station.Type,
                                     GroupCodes = (from x in context.StationGroupCodes where x.StationId == station.Id select new OptionDataModel() { id = x.GroupCode, text = x.GroupCode }).ToList(),
                                     StationGroupCodes = (from x in context.StationGroupCodes where x.StationId == station.Id select x).ToList(),
                                     Nodes = (from x in context.StationNodes where x.StationId == station.Id select new OptionDataModel() { id = x.NodeId, text = x.NodeId }).ToList(),
                                     StationNodes = (from x in context.StationNodes where x.StationId == station.Id select x).ToList(),
                                     CreatedAt = station.CreatedAt,
                                     CreatedUser = station.CreatedUser,
                                     UpdatedAt = station.UpdatedAt,
                                     UpdatedUser = station.UpdatedUser,
                                     DeletedAt = station.DeletedAt,
                                     DeletedUser = station.DeletedUser,
                                 };
                    return (filter == null) ? result.ToList() : result.Where(filter).ToList();
                }
            }
            public override Station? Get(Expression<Func<Station, bool>> filter)
            {
                return GetAll(filter).FirstOrDefault();
            }
        }
        public IDataResult<IPaginationResult<List<Station>>> GetAllWithPagination(IPageableFilter<Station>? pageableFilter = null)
        {
            return new SuccessDataResult<IPaginationResult<List<Station>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<Station>> GetAll(Expression<Func<Station, bool>>? filter = null)
        {
            return new SuccessDataResult<List<Station>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<Station?> Get(Expression<Func<Station, bool>> filter)
        {
            var result = _repository.Get(filter);
            result.InitializeView();
            return new SuccessDataResult<Station?>(result, "Get filter data");
        }
        public ISingleResult Add()
        {
            Type = TypeView?.id.ToString() ?? Type;
            _repository.Add(this);
            Nodes?.ForEach(item =>
            {
                new StationNode() { StationId = Id, NodeId = item.id.ToString() }.Add();
            });
            GroupCodes?.ForEach(item =>
            {
                new StationGroupCode() { StationId = Id, GroupCode = item.id.ToString() }.Add();
            });
            return new SuccessResult("Added");
        }
        public ISingleResult Update()
        {
            Type = TypeView?.id.ToString() ?? Type;
            _repository.Update(this);
            Nodes?.ForEach(item =>
            {
                new StationNode() { StationId = Id, NodeId = item.id.ToString() }.CheckAndAdd();
            });
            StationNodes?.ForEach(item =>
            {
                item.Update();
            });
            new StationNode().GetAll(e => e.StationId == Id).Data.Where(x => !(Nodes?.Select(y => y.id.ToString()).ToList().Contains(x.NodeId) ?? false)).ToList().ForEach(item =>
            {
                item.Delete();
            });
            GroupCodes?.ForEach(item =>
            {
                new StationGroupCode() { StationId = Id, GroupCode = item.id.ToString() }.CheckAndAdd();
            });
            new StationGroupCode().GetAll(e => e.StationId == Id).Data.Where(x => !(GroupCodes?.Select(y => y.id.ToString()).ToList().Contains(x.GroupCode) ?? false)).ToList().ForEach(item =>
            {
                item.Delete();
            });
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
        public async Task<IDataResult<IPaginationResult<List<Station>>>> GetAllWithPaginationAsync(IPageableFilter<Station>? pageableFilter = null)
        {
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<Station>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<Station>>> GetAllAsync(Expression<Func<Station, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<Station>>(result, "Listed all data");
        }
        public async Task<IDataResult<Station?>> GetAsync(Expression<Func<Station, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            result.InitializeView();
            return new SuccessDataResult<Station?>(result, "Get filter data");
        }
        public async Task<ISingleResult> AddAsync()
        {
            Type = TypeView?.id.ToString() ?? Type;
            await _repository.AddAsync(this);
            Nodes?.ForEach(item =>
            {
                new StationNode() { StationId = Id, NodeId = item.id.ToString() }.Add();
            });
            GroupCodes?.ForEach(item =>
            {
                new StationGroupCode() { StationId = Id, GroupCode = item.id.ToString() }.Add();
            });
            return new SuccessResult("Added");
        }
        public async Task<ISingleResult> UpdateAsync()
        {
            Type = TypeView?.id.ToString() ?? Type;
            await _repository.UpdateAsync(this);
            Nodes?.ForEach(item =>
            {
                new StationNode() { StationId = Id, NodeId = item.id.ToString() }.CheckAndAdd();
            });
            new StationNode().GetAll(e => e.StationId == Id).Data.Where(x => !(Nodes?.Select(y => y.id.ToString()).ToList().Contains(x.NodeId) ?? false)).ToList().ForEach(item =>
            {
                item.Delete();
            });
            GroupCodes?.ForEach(item =>
            {
                new StationGroupCode() { StationId = Id, GroupCode = item.id.ToString() }.CheckAndAdd();
            });
            new StationGroupCode().GetAll(e => e.StationId == Id).Data.Where(x => !(GroupCodes?.Select(y => y.id.ToString()).ToList().Contains(x.GroupCode) ?? false)).ToList().ForEach(item =>
            {
                item.Delete();
            });
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

        public IDataResult<IPaginationResult<List<Station>>> GetAllSelectedByAuthorityId(int authorityId, IPageableFilter<Station> pageableFilter)
        {
            var claimIds = new AuthorityStation().GetAll(e => e.AuthorityId == authorityId).Data.Select(e => e.StationId);
            return new SuccessDataResult<IPaginationResult<List<Station>>>(_repository.GetAllWithPagination(new PageableFilter<Station>(pageableFilter.Filter.And(e => claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by in authority data");

        }

        public IDataResult<IPaginationResult<List<Station>>> GetAllUnSelectedByAuthorityId(int authorityId, IPageableFilter<Station> pageableFilter)
        {
            var claimIds = new AuthorityStation().GetAll(e => e.AuthorityId == authorityId).Data.Select(e => e.StationId);
            return new SuccessDataResult<IPaginationResult<List<Station>>>(_repository.GetAllWithPagination(new PageableFilter<Station>(pageableFilter.Filter.And(e => !claimIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by not in authority data");
        }
        public ISingleResult SaveSelectedByAuthorityId(int authorityId, List<int> claimIds)
        {
            var dbClaims = new AuthorityStation().GetAll(e => e.AuthorityId == authorityId).Data;
            claimIds.Where(c => !dbClaims.Select(e => e.StationId).Contains(c)).ToList().ForEach(id => new AuthorityStation() { AuthorityId = authorityId, StationId = id }.Add());
            dbClaims.Where(c => !claimIds.Contains(c.StationId)).ToList().ForEach(claim => claim.Delete());
            return new SuccessResult("List saved");
        }
        public IDataResult<IPaginationResult<List<Station>>> GetAllSelectedByUserId(int userId, IPageableFilter<Station> pageableFilter)
        {
            //var claimIds = new UserStation().GetAll(e => e.UserId == userId).Data.Select(e => e.StationId);
            var stationIds = new User().Get(u => u.Id == userId).Data?.GetStations().Select(e => e.Id);
            return new SuccessDataResult<IPaginationResult<List<Station>>>(_repository.GetAllWithPagination(new PageableFilter<Station>(pageableFilter.Filter.And(e => stationIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by in user data");

        }
        public IDataResult<List<Station>> GetAllByUserId(int userId)
        {
            //var claimIds = new UserStation().GetAll(e => e.UserId == userId).Data.Select(e => e.StationId);
            var stationIds = new User().Get(u => u.Id == userId).Data?.GetStations().Select(e => e.Id);
            return new SuccessDataResult<List<Station>>(_repository.GetAll(e => stationIds.Contains(e.Id)), "Listed all selected by in user data");

        }

        public IDataResult<IPaginationResult<List<Station>>> GetAllUnSelectedByUserId(int userId, IPageableFilter<Station> pageableFilter)
        {
            //var claimIds = new UserStation().GetAll(e => e.UserId == userId).Data.Select(e => e.StationId);
            var stationIds = new User().Get(u => u.Id == userId).Data?.GetStations().Select(e => e.Id);
            return new SuccessDataResult<IPaginationResult<List<Station>>>(_repository.GetAllWithPagination(new PageableFilter<Station>(pageableFilter.Filter.And(e => !stationIds.Contains(e.Id)), pageableFilter.Pagination)), "Listed all selected by not in user data");
        }
        public ISingleResult SaveSelectedByUserId(int userId, List<int> claimIds)
        {
            var dbClaims = new UserStation().GetAll(e => e.UserId == userId).Data;
            claimIds.Where(c => !dbClaims.Select(e => e.StationId).Contains(c)).ToList().ForEach(id => new UserStation() { UserId = userId, StationId = id }.Add());
            dbClaims.Where(c => !claimIds.Contains(c.StationId)).ToList().ForEach(claim => claim.Delete());
            return new SuccessResult("List saved");
        }
        private void InitializeView()
        {
            if (Type != null) TypeView = new OptionDataModel() { id = Type, text = Type };
        }
    }
}
