using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Entities.DTOs;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.IoC;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Core.Utilities.Security;
using ZgnWebApi.DataAccess.Contexts;

namespace ZgnWebApi.Entities
{
    public class User : BaseTimeStamp, IPageableEntity<User>
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [NotMapped]
        public OptionDataModel? TypeView { get; set; }
        public string? Type { get; set; }
        public string? UserName { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }
        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }
        public bool Status { get; set; }
        public bool Banned { get; set; }
        public string? BannedMsg { get; set; }
        public DateTime? ConnectionIat { get; set; }

        [NotMapped]
        public readonly Repository _repository = new Repository();
        private readonly ITokenHelper _tokenHelper;

        public User()
        {
            _tokenHelper = ServiceTool.ServiceProvider.GetService<ITokenHelper>();
        }

        public class Repository : EfBaseRepository<User, ZgnAgvManagerContext>
        {
            public List<OperationClaim> GetClaims(User entity)
            {
                using ZgnAgvManagerContext context = new();
                var result =
                            (from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == entity.Id
                             select operationClaim).Concat(
                                (from operationClaim in context.OperationClaims
                                 join authorityOperationClaim in context.AuthorityOperationClaims on operationClaim.Id equals authorityOperationClaim.OperationClaimId
                                 join entityAuthority in context.UserAuthorities on authorityOperationClaim.AuthorityId equals entityAuthority.AuthorityId
                                 where entityAuthority.UserId == entity.Id
                                 select operationClaim));
                return result.ToList();
            }
            public List<Station> GetStations(User entity)
            {
                using ZgnAgvManagerContext context = new();
                var result =
                            (from station in context.Stations
                             join userStation in context.UserStations on station.Id equals userStation.StationId
                             where userStation.UserId == entity.Id
                             select station).Concat(
                                (from station in context.Stations
                                 join authorityStation in context.AuthorityStations on station.Id equals authorityStation.StationId
                                 join userAuthority in context.UserAuthorities on authorityStation.AuthorityId equals userAuthority.AuthorityId
                                 where userAuthority.UserId == entity.Id
                                 select station));
                return result.ToList();
            }
        }
        public IDataResult<AccessToken> CreateAccessToken()
        {
            var accessToken = _tokenHelper.CreateToken(this, GetClaims());
            accessToken.Type = Type;
            ConnectionIat = DateTime.Now;
            accessToken.FullName = FirstName + " " + LastName;
            Update();
            return new SuccessDataResult<AccessToken>(accessToken, "tokenCreated");
        }
        public List<OperationClaim> GetClaims()
        {
            return _repository.GetClaims(this);
        }
        public List<Station> GetStations()
        {
            return _repository.GetStations(this);
        }
        public IDataResult<IPaginationResult<List<User>>> GetAllWithPagination(IPageableFilter<User>? pageableFilter = null)
        {
            pageableFilter.Filter = (ServiceTool.GetUserId() > 1) ? pageableFilter.Filter.And(e => e.Id > 1) : pageableFilter.Filter;
            return new SuccessDataResult<IPaginationResult<List<User>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<User>> GetAll(Expression<Func<User, bool>>? filter = null)
        {
            filter = (ServiceTool.GetUserId() > 1) ? filter.And(e => e.Id > 1) : filter;
            return new SuccessDataResult<List<User>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<User?> Get(Expression<Func<User, bool>> filter)
        {
            filter = (ServiceTool.GetUserId() > 1) ? filter.And(e => e.Id > 1) : filter;
            var u = _repository.Get(filter);
            u?.InitializeView();
            return new SuccessDataResult<User?>(u, "Get filter data");
        }
        public ISingleResult Add()
        {
            Type = TypeView?.id.ToString() ?? Type;
            if (Password == null || Password == string.Empty) Password = "1234";
            HashingHelper.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Status = true;
            _repository.Add(this);
            return new SuccessResult("Added");
        }
        public ISingleResult CheckAndAdd()
        {
            if (_repository.Get(e => e.UserName == UserName) == null)
            {
                Add();
            }
            return new SuccessResult("Checked and added");
        }
        public ISingleResult Update()
        {
            Type = TypeView?.id.ToString() ?? Type;
            if (Password != null && Password != string.Empty)
            {
                HashingHelper.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
                PasswordHash = passwordHash;
                PasswordSalt = passwordSalt;
            }
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
        public async Task<IDataResult<IPaginationResult<List<User>>>> GetAllWithPaginationAsync(IPageableFilter<User>? pageableFilter = null)
        {
            pageableFilter.Filter = (ServiceTool.GetUserId() > 1) ? pageableFilter.Filter.And(e => e.Id > 1) : pageableFilter.Filter;
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<User>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<User>>> GetAllAsync(Expression<Func<User, bool>>? filter = null)
        {
            filter = (ServiceTool.GetUserId() > 1) ? filter.And(e => e.Id > 1) : filter;
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<User>>(result, "Listed all data");
        }
        public async Task<IDataResult<User?>> GetAsync(Expression<Func<User, bool>> filter)
        {
            filter = (ServiceTool.GetUserId() > 1) ? filter.And(e => e.Id > 1) : filter;
            var result = await _repository.GetAsync(filter);
            result?.InitializeView();
            return new SuccessDataResult<User?>(result, "Get filter data");
        }
        public async Task<ISingleResult> AddAsync()
        {
            Type = TypeView?.id.ToString() ?? Type;
            if (Password == null || Password == string.Empty) Password = "1234";
            HashingHelper.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Status = true;
            await _repository.AddAsync(this);
            return new SuccessResult("Added");
        }
        public async Task<ISingleResult> UpdateAsync()
        {
            Type = TypeView?.id.ToString() ?? Type;
            if (Password != null && Password != string.Empty)
            {
                HashingHelper.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
                PasswordHash = passwordHash;
                PasswordSalt = passwordSalt;
            }
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
        private void InitializeView()
        {
            if (Type != null) TypeView = new OptionDataModel() { id = Type, text = Type };
        }

    }
}
