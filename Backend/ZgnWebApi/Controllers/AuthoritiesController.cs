using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using ZgnWebApi.Controllers.Base;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Helpers;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Entities;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoritiesController : PageableController<Authority>
    {
        public AuthoritiesController()
        {
            base.AddRole("Authority.GetAll");
            base.AddRole("Authority.Get");
            base.AddRole("Authority.Add");
            base.AddRole("Authority.Update");
            base.AddRole("Authority.Delete");
            base.AddRole("Authority.SoftDelete");
            base.AddRole("Authority.GetAllSelectedByUserId");
            base.AddRole("Authority.GetAllUnSelectedByUserId");
            base.AddRole("Authority.SaveSelectedByUserId");
        }
        public override IActionResult GetAll()
        {
            base.CheckRole("Authority.GetAll");
            return base.GetAll();
        }
        public override IActionResult GetById(int id)
        {
            base.CheckRole("Authority.Get");
            return base.GetById(id);
        }
        public override IActionResult Add(Authority entity)
        {
            base.CheckRole("Authority.Add");
            return base.Add(entity);
        }
        public override IActionResult Update(Authority entity)
        {
            base.CheckRole("Authority.Update");
            return base.Update(entity);
        }
        public override IActionResult Delete(Authority entity)
        {
            base.CheckRole("Authority.Delete");
            return base.Delete(entity);
        }
        public override IActionResult GetAllForUi(string? request)
        {
            base.CheckRole("Authority.GetAll");
            return base.GetAllForUi(request);
        }
        public override IActionResult GetForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Authority.Get");
            return base.GetForUi(data);
        }
        public override IActionResult AddForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Authority.Add");
            return base.AddForUi(data);
        }
        public override IActionResult UpdateForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Authority.Update");
            return base.UpdateForUi(data);
        }
        public override IActionResult DeleteForUi(string request)
        {
            base.CheckRole("Authority.Delete");
            return base.DeleteForUi(request);
        }
        public override IActionResult SoftDeleteForUi(string request)
        {
            base.CheckRole("Authority.SoftDelete");
            return base.SoftDeleteForUi(request);
        }
        [HttpGet("GetAllSelectedByUserIdUi")]
        public IActionResult GetAllSelectedByUserId(int UserId, string request)
        {
            base.CheckRole("Authority.GetAllSelectedByUserId");
            IPageableFilter<Authority> operationClaimRequest = RequestHelper.GetRequestByGridExpression<Authority>(request);
            var result = new Authority().GetAllSelectedByUserId(UserId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Authority> successResult = new SuccessGridResult<Authority>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllUnSelectedByUserIdUi")]
        public IActionResult GetAllUnSelectedByUserIdUi(int UserId, string request)
        {
            base.CheckRole("Authority.GetAllUnSelectedByUserId");
            IPageableFilter<Authority> operationClaimRequest = RequestHelper.GetRequestByGridExpression<Authority>(request);
            var result = new Authority().GetAllUnSelectedByUserId(UserId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Authority> successResult = new SuccessGridResult<Authority>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpPost("SaveSelectedByUserIdUi")]
        public IActionResult SaveSelectedByUserIdUi(int UserId, [FromForm] HandleRequestData data)
        {
            base.CheckRole("Authority.SaveSelectedByUserId");
            dynamic json = JsonConvert.DeserializeObject(data.Request);
            List<int> Ids = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(json["list"]));
            var result = new Authority().SaveSelectedByUserId(UserId, Ids);
            if (result.Success)
            {
                return Ok(result);
            }
            var errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
    }
}
