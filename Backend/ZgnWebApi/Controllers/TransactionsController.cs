using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ZgnWebApi.Controllers.Base;
using ZgnWebApi.Core.Entities.DTOs;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Helpers;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Entities;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : PageableController<Transaction>
    {
        public TransactionsController()
        {
            base.AddRole("Transaction.GetAll");
            base.AddRole("Transaction.Get");
            base.AddRole("Transaction.Add");
            base.AddRole("Transaction.GetAllPending");
            base.AddRole("Transaction.GetMissionByTransactionId");
        }

        public override IActionResult GetAll()
        {
            base.CheckRole("Transaction.GetAll");
            return base.GetAll();
        }
        public override IActionResult GetById(int id)
        {
            base.CheckRole("Transaction.Get");
            return base.GetById(id);
        }
        public override IActionResult Add(Transaction entity)
        {
            base.CheckRole("Transaction.Add");
            var result=base.Add(entity);
            return result;
        }
        public override IActionResult Update(Transaction entity)
        {
            base.CheckRole("");
            return base.Update(entity);
        }
        public override IActionResult Delete(Transaction entity)
        {
            base.CheckRole("");
            return base.Delete(entity);
        }
        public override IActionResult GetAllForUi(string? request)
        {
            base.CheckRole("Transaction.GetAll");
            return base.GetAllForUi(request);
        }
        [HttpGet("GetAllPendingForUi")]
        public IActionResult GetAllPendingForUi(string? request)
        {
            base.CheckRole("Transaction.GetAllPending");
            IPageableFilter<Transaction> userRequest = RequestHelper.GetRequestByGridExpression<Transaction>(request);
            userRequest.Filter = userRequest.Filter.And(e => e.DeletedAt == null&&e.Status==TransactionStatus.Pending.Value);
            userRequest.Key += "AND (e.DeletedAt == null) AND (e.Status==\"Pending\")";
            var result = new Transaction().GetAllWithPagination(userRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Transaction> successResult = new SuccessGridResult<Transaction>(data, userRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        public override IActionResult GetForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Transaction.Get");
            return base.GetForUi(data);
        }
        public override IActionResult AddForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Transaction.Add");
            return base.AddForUi(data);
        }
        [HttpPost("StartForUi")]
        public IActionResult StartForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Transaction.Start");
            RequestSaveByFormModel<Transaction> userRequest = RequestHelper.SaveRequestByForm<Transaction>(data.Request);
            var record = userRequest.Record.Get(u => u.Id == userRequest.Record.Id).Data;
            record.FromNode = userRequest.Record.FromNode;
            record.FromNodeView = userRequest.Record.FromNodeView;
            var result = record.Start();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Start")]
        public IActionResult Start(Transaction transaction)
        {
            base.CheckRole("Transaction.Start");
            var result = transaction.Start();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetMissionByTransactionIdUi")]
        public IActionResult GetMissionByTransactionIdUi(int TransactionId)
        {
            base.CheckRole("Transaction.GetMissionByTransactionId");
            var result = new Transaction().Get(e => e.Id == TransactionId);
            if (result.Success)
            {
                var data = result.Data.GetMission();
                ISuccessGridResult<KeyValueResult> successResult = new SuccessGridResult<KeyValueResult>(data.Data, 0, data.Data.Count);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        public override IActionResult UpdateForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("");
            return base.UpdateForUi(data);
        }
        public override IActionResult DeleteForUi(string request)
        {
            base.CheckRole("");
            return base.DeleteForUi(request);
        }
        public override IActionResult SoftDeleteForUi(string request)
        {
            base.CheckRole("");
            return base.SoftDeleteForUi(request);
        }
    }
}
