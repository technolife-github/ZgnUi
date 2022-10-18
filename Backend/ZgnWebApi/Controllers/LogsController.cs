using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZgnWebApi.Controllers.Base;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Helpers;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Entities;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : PageableController<Log>
    {
        public LogsController()
        {
            base.AddRole("Log.GetAll");
            base.AddRole("Log.Get");
            base.AddRole("Log.Add");
            base.AddRole("Log.Update");
            base.AddRole("Log.Delete");
            base.AddRole("Log.SoftDelete");
            base.AddRole("Log.GetAllByTransactionId");
        }
        public override IActionResult GetAll()
        {
            base.CheckRole("Log.GetAll");
            return base.GetAll();
        }
        public override IActionResult GetById(int id)
        {
            base.CheckRole("Log.Get");
            return base.GetById(id);
        }
        public override IActionResult Add(Log entity)
        {
            base.CheckRole("Log.Add");
            return base.Add(entity);
        }
        public override IActionResult Update(Log entity)
        {
            base.CheckRole("Log.Update");
            return base.Update(entity);
        }
        public override IActionResult Delete(Log entity)
        {
            base.CheckRole("Log.Delete");
            return base.Delete(entity);
        }
        public override IActionResult GetAllForUi(string? request)
        {
            base.CheckRole("Log.GetAll");
            return base.GetAllForUi(request);
        }
        public override IActionResult GetForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Log.Get");
            return base.GetForUi(data);
        }
        public override IActionResult AddForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Log.Add");
            return base.AddForUi(data);
        }
        public override IActionResult UpdateForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Log.Update");
            return base.UpdateForUi(data);
        }
        public override IActionResult DeleteForUi(string request)
        {
            base.CheckRole("Log.Delete");
            return base.DeleteForUi(request);
        }
        public override IActionResult SoftDeleteForUi(string request)
        {
            base.CheckRole("Log.SoftDelete");
            return base.SoftDeleteForUi(request);
        }
        [HttpGet("GetAllByTransactionIdUi")]
        public IActionResult GetAllByTransactionIdUi(int TransactionId, string request)
        {
            base.CheckRole("Log.GetAllByTransactionId");
            IPageableFilter<Log> logRequest = RequestHelper.GetRequestByGridExpression<Log>(request);
            var result = new Log().GetAllByTransactionIdUi(TransactionId, logRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Log> successResult = new SuccessGridResult<Log>(data, logRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }


    }
}
