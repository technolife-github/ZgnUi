using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Transactions;
using ZgnWebApi.Core.DataAccess.EntityFramework;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Entities.DTOs;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.IoC;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.DataAccess.Contexts;
using ZgnWebApi.Integrations.BlueBotics;

namespace ZgnWebApi.Entities
{
    public class Transaction : BaseTimeStamp, IPageableEntity<Transaction>
    {
        public int Id { get; set; }
        public string? TransactionType { get; set; }//TALEP,IADE
        public string? Status { get; set; }//Pending,Ready,End
        [NotMapped]
        public OptionDataModel? FromNodeView { get; set; }
        public string? FromNode { get; set; }
        [NotMapped]
        public OptionDataModel? ToNodeView { get; set; }
        public string? ToNode { get; set; }
        public string? ProcessId { get; set; }
        [NotMapped]
        public OptionDataModel? GroupCodeView { get; set; }
        public string? GroupCode { get; set; }
        [NotMapped]
        public OptionDataModel? ProductIdView { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public OptionDataModel? ProductCodeView { get; set; }
        public string? ProductCode { get; set; }
        [NotMapped]
        public OptionDataModel? SerialNumberView { get; set; }
        public string? SerialNumber { get; set; }
        [NotMapped]
        public OptionDataModel? LocationNameView { get; set; }
        public string? LocationName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [NotMapped]
        private readonly IBlueBoticsIntegration _blueBoticsIntegration;
        public Transaction()
        {
            _blueBoticsIntegration = ServiceTool.ServiceProvider.GetService<IBlueBoticsIntegration>();
        }
        [NotMapped]
        public readonly Repository _repository = new Repository();
        public class Repository : EfBaseRepository<Transaction, ZgnAgvManagerContext>
        {
        }
        public IDataResult<IPaginationResult<List<Transaction>>> GetAllWithPagination(IPageableFilter<Transaction>? pageableFilter = null)
        {
            return new SuccessDataResult<IPaginationResult<List<Transaction>>>(_repository.GetAllWithPagination(pageableFilter), "Listed all data");
        }
        public IDataResult<List<Transaction>> GetAll(Expression<Func<Transaction, bool>>? filter = null)
        {
            return new SuccessDataResult<List<Transaction>>(_repository.GetAll(filter), "Listed all data");
        }
        public IDataResult<Transaction?> Get(Expression<Func<Transaction, bool>> filter)
        {
            return new SuccessDataResult<Transaction?>(_repository.Get(filter), "Get filter data");
        }
        public ISingleResult Add()
        {
            FromNode = FromNodeView?.id.ToString() ?? FromNode;
            ToNode = ToNodeView?.id.ToString() ?? ToNode;
            GroupCode = GroupCodeView?.id.ToString() ?? GroupCode;
            SerialNumber = SerialNumberView?.id.ToString() ?? SerialNumber;
            ProductCode = ProductCodeView?.id.ToString() ?? ProductCode;
            LocationName = LocationNameView?.id.ToString() ?? LocationName;
            ProductId = int.Parse(ProductIdView?.id.ToString() ?? ProductId.ToString());
            Status = TransactionStatus.Pending.Value;
            StartDate = DateTime.Now;
            EndDate = null;
            CreatedAt = DateTime.Now;
            CreatedUser = ServiceTool.GetUserId();
            _repository.Add(this);
            new Log(Id, LogType.Info, "İşlem kaydı başlatıldı.").Add();
            return new SuccessResult("Added");
        }
        public ISingleResult Update()
        {
            FromNode = FromNodeView?.id.ToString() ?? FromNode;
            ToNode = ToNodeView?.id.ToString() ?? ToNode;
            GroupCode = GroupCodeView?.id.ToString() ?? GroupCode;
            SerialNumber = SerialNumberView?.id.ToString() ?? SerialNumber;
            ProductCode = ProductCodeView?.id.ToString() ?? ProductCode;
            LocationName = LocationNameView?.id.ToString() ?? LocationName;
            ProductId = int.Parse(ProductIdView?.id.ToString() ?? ProductId.ToString());
            _repository.Update(this);
            return new SuccessResult("Updated");
        }
        public ISingleResult UpdateStatus()
        {
            new Log(Id, LogType.Info, $"İşleme ait son durum: {Status}").Add() ;
            if (Status == TransactionStatus.Terminated.Value)
            {
                Status = TransactionStatus.End.Value;
                EndDate = DateTime.Now;
                new Log(Id, LogType.Info, $"İşleme tamamlandı.").Add();
            }
            if (Status == TransactionStatus.Cancelled.Value)
            {
                EndDate = DateTime.Now;
                new Log(Id, LogType.Info, $"İşlemden vazgeçildi.").Add();
            }
            if (Status == TransactionStatus.Cancelled.Value)
            {
                EndDate = DateTime.Now;
                new Log(Id, LogType.Info, $"İşlem sırasında bir hata ile karşılaşıldı.").Add();
            }
            _repository.Update(this);
            return new SuccessResult("Updated");
        }
        public ISingleResult Start(bool isAdd=false)
        {
            using var transactionScope = new TransactionScope();
            try
            {
                if (ProcessId != null)
                {
                    return new ErrorResult("İşlem zaten başlatılmış.");
                }
                FromNode = FromNodeView?.id.ToString() ?? FromNode;
                ToNode = ToNodeView?.id.ToString() ?? ToNode;
                GroupCode = GroupCodeView?.id.ToString() ?? GroupCode;
                SerialNumber = SerialNumberView?.id.ToString() ?? SerialNumber;
                ProductCode = ProductCodeView?.id.ToString() ?? ProductCode;
                LocationName = LocationNameView?.id.ToString() ?? LocationName;
                ProductId = int.Parse(ProductIdView?.id.ToString() ?? ProductId.ToString());
                var result = _blueBoticsIntegration.AddMission(FromNode, ToNode);
                if (FromNode == null)
                {
                    return new ErrorResult("Alış Noktası boş olamaz.");
                }
                if (result.RetCode != 0)
                {
                    throw new Exception(result.Error);
                }
                ProcessId = result?.Payload?.Acceptedmissions[0];
                if (ProcessId == null)
                {
                    throw new Exception("ProcessId null");
                }
                Status = TransactionStatus.Ready.Value;
                _repository.Update(this);
                var mission = _blueBoticsIntegration.GetMission(ProcessId);
                if (mission == null) throw new Exception("Görev oluşturulamadı");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (mission.Payload == null && stopwatch.Elapsed.TotalSeconds<15)
                {
                    Thread.Sleep(100);
                    mission = _blueBoticsIntegration.GetMission(ProcessId);
                }
                stopwatch.Stop();
                new Log(Id, LogType.Info, "İşlem başlatıldı. ProcessId: " + ProcessId).Add();
                new Log(Id, LogType.Info, "Tahmini işlem bitiş zamanı: " + mission.Payload.Deadline?.ToString("yyyy-MM-dd HH:mm:ss")).Add();
                transactionScope.Complete();

            }
            catch (Exception e)
            {
                new Log(Id, LogType.Error, e.Message).Add();
                transactionScope.Dispose();
                throw e;
            }
            return new SuccessResult("Updated");
        }
        public IDataResult<List<KeyValueResult>> GetMission()
        {
            if (ProcessId == null) return new ErrorDataResult<List<KeyValueResult>>("İşlem bulunamadı.");
            var response = _blueBoticsIntegration.GetMission(ProcessId);
            if(response.Payload==null) return new ErrorDataResult<List<KeyValueResult>>("İşlem bulunamadı.");
            var result = new List<KeyValueResult>().InitializeMission(response.Payload);
            return new SuccessDataResult<List<KeyValueResult>>(result);
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
        public async Task<IDataResult<IPaginationResult<List<Transaction>>>> GetAllWithPaginationAsync(IPageableFilter<Transaction>? pageableFilter = null)
        {
            var result = await _repository.GetAllWithPaginationAsync(pageableFilter);
            return new SuccessDataResult<IPaginationResult<List<Transaction>>>(result, "Listed all data");
        }
        public async Task<IDataResult<List<Transaction>>> GetAllAsync(Expression<Func<Transaction, bool>>? filter = null)
        {
            var result = await _repository.GetAllAsync(filter);
            return new SuccessDataResult<List<Transaction>>(result, "Listed all data");
        }
        public async Task<IDataResult<Transaction?>> GetAsync(Expression<Func<Transaction, bool>> filter)
        {
            var result = await _repository.GetAsync(filter);
            return new SuccessDataResult<Transaction?>(result, "Get filter data");
        }
        public async Task<ISingleResult> AddAsync()
        {
            FromNode = FromNodeView?.id.ToString() ?? FromNode;
            ToNode = ToNodeView?.id.ToString() ?? ToNode;
            GroupCode = GroupCodeView?.id.ToString() ?? GroupCode;
            SerialNumber = SerialNumberView?.id.ToString() ?? SerialNumber;
            ProductCode = ProductCodeView?.id.ToString() ?? ProductCode;
            LocationName = LocationNameView?.id.ToString() ?? LocationName;
            ProductId = int.Parse(ProductIdView?.id.ToString() ?? ProductId.ToString());
            Status = TransactionStatus.Pending.Value;
            StartDate = DateTime.Now;
            EndDate = null;
            CreatedAt = DateTime.Now;
            CreatedUser = ServiceTool.GetUserId();
            await _repository.AddAsync(this);
            new Log(Id, LogType.Info, "İşlem kaydı başlatıldı.").Add();
            return new SuccessResult("Added");
        }
        public async Task<ISingleResult> UpdateAsync()
        {
            FromNode = FromNodeView?.id.ToString() ?? FromNode;
            ToNode = ToNodeView?.id.ToString() ?? ToNode;
            GroupCode = GroupCodeView?.id.ToString() ?? GroupCode;
            SerialNumber = SerialNumberView?.id.ToString() ?? SerialNumber;
            ProductCode = ProductCodeView?.id.ToString() ?? ProductCode;
            LocationName = LocationNameView?.id.ToString() ?? LocationName;
            ProductId = int.Parse(ProductIdView?.id.ToString() ?? ProductId.ToString());
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
