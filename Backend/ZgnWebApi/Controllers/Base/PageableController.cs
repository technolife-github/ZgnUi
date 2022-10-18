using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZgnWebApi.Core.Entities;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Helpers;
using ZgnWebApi.Core.Utilities.Results;

namespace ZgnWebApi.Controllers.Base
{
    public abstract class PageableController<T> : BaseController<T>
        where T : class, IPageableEntity<T>, new()
    {
        [HttpGet("GetAllForUi")]
        public virtual IActionResult GetAllForUi(string? request)
        {
            IPageableFilter<T> userRequest = RequestHelper.GetRequestByGridExpression<T>(request);
            userRequest.Filter = userRequest.Filter.And(e => e.DeletedAt == null);
            userRequest.Key += "AND (e.DeletedAt == null)";
            var result = new T().GetAllWithPagination(userRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<T> successResult = new SuccessGridResult<T>(data, userRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllDeletedForUi")]
        public virtual IActionResult GetAllDeletedForUi(string request)
        {
            IPageableFilter<T> userRequest = RequestHelper.GetRequestByGridExpression<T>(request);
            userRequest.Filter = userRequest.Filter.And(e => e.DeletedAt != null);
            userRequest.Key += "AND (e.DeletedAt != null)";
            var result = new T().GetAllWithPagination(userRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<T> successResult = new SuccessGridResult<T>(data, userRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }

        [HttpPost("GetForUi")]
        public virtual IActionResult GetForUi([FromForm] HandleRequestData data)
        {
            dynamic json = JsonConvert.DeserializeObject(data.Request);
            int id = json.recid;
            var result = new T().Get(e => e.Id == id);
            if (result.Success)
            {
                ISuccessFormResult<T> successResult = new SuccessFormResult<T>(result.Data, "Get data successfully");
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);

        }
        [HttpPost("AddForUi")]
        public virtual IActionResult AddForUi([FromForm] HandleRequestData data)
        {
            RequestSaveByFormModel<T> userRequest = RequestHelper.SaveRequestByForm<T>(data.Request);
            var result = userRequest.Record.Add();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("UpdateForUi")]
        public virtual IActionResult UpdateForUi([FromForm] HandleRequestData data)
        {
            RequestSaveByFormModel<T> userRequest = RequestHelper.SaveRequestByForm<T>(data.Request);
            var record = userRequest.Record.Get(u => u.Id == userRequest.Record.Id).Data;
            record = (T)EntityHelper.Copy(userRequest.Record, record);
            var result = record.Update();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("DeleteForUi")]
        public virtual IActionResult DeleteForUi(string request)
        {
            List<int> list = RequestHelper.DeleteRequestByGrid<List<int>>(request, "Id");
            var users = new T().GetAll(s => list.Contains(s.Id)).Data;
            ISingleResult result = new SuccessResult("Deleted");
            users.ForEach(u =>
            {
                result = u.Delete();
            });
            return Ok();
        }

        [HttpGet("SoftDeleteForUi")]
        public virtual IActionResult SoftDeleteForUi(string request)
        {
            List<int> list = RequestHelper.DeleteRequestByGrid<List<int>>(request, "Id");
            var users = new T().GetAll(s => list.Contains(s.Id)).Data;
            ISingleResult result = new SuccessResult("Deleted");
            users.ForEach(u =>
            {
                result = u.SoftDelete();
            });
            return Ok(new SuccessResult("Silme işlemi başarılı"));
        }

        [HttpGet("SaveForUi")]
        public virtual IActionResult SaveForUi(string request)
        {
            RequestSaveByGridModel<T> userRequest = RequestHelper.SaveRequestByGrid<T>(request);
            int error = 0;
            userRequest.changes.ForEach((Action<T>)(item =>
            {
                var record = item.Get(u => u.Id == item.Id).Data;
                record = (T)EntityHelper.Copy(item, record);
                var result = record.Update();
                if (!result.Success)
                {
                    error++;
                }
            }));
            return (error == 0) ? Ok(new SuccessResult("Saved all changes")) : BadRequest(new ErrorResult("Not saved all data. Try again"));
        }
    }
}
