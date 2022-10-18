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
    public class OperationClaimsController : PageableController<OperationClaim>
    {
        public OperationClaimsController()
        {
            base.AddRole("OperationClaim.GetAll");
            base.AddRole("OperationClaim.Get");
            base.AddRole("OperationClaim.Add");
            base.AddRole("OperationClaim.Update");
            base.AddRole("OperationClaim.Delete");
            base.AddRole("OperationClaim.SoftDelete");
            base.AddRole("OperationClaim.GetAllSelectedByUserId");
            base.AddRole("OperationClaim.GetAllUnSelectedByUserId");
            base.AddRole("OperationClaim.SaveSelectedByUserId");
            base.AddRole("OperationClaim.GetAllSelectedByAuthorityId");
            base.AddRole("OperationClaim.GetAllUnSelectedByAuthorityId");
            base.AddRole("OperationClaim.SaveSelectedByAuthorityId");
        }
        public override IActionResult GetAll()
        {
            base.CheckRole("OperationClaim.GetAll");
            return base.GetAll();
        }
        public override IActionResult GetById(int id)
        {
            base.CheckRole("OperationClaim.Get");
            return base.GetById(id);
        }
        public override IActionResult Add(OperationClaim entity)
        {
            base.CheckRole("OperationClaim.Add");
            return base.Add(entity);
        }
        public override IActionResult Update(OperationClaim entity)
        {
            base.CheckRole("OperationClaim.Update");
            return base.Update(entity);
        }
        public override IActionResult Delete(OperationClaim entity)
        {
            base.CheckRole("OperationClaim.Delete");
            return base.Delete(entity);
        }
        public override IActionResult GetAllForUi(string? request)
        {
            base.CheckRole("OperationClaim.GetAll");
            return base.GetAllForUi(request);
        }
        public override IActionResult GetForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("OperationClaim.Get");
            return base.GetForUi(data);
        }
        public override IActionResult AddForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("OperationClaim.Add");
            return base.AddForUi(data);
        }
        public override IActionResult UpdateForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("OperationClaim.Update");
            return base.UpdateForUi(data);
        }
        public override IActionResult DeleteForUi(string request)
        {
            base.CheckRole("OperationClaim.Delete");
            return base.DeleteForUi(request);
        }
        public override IActionResult SoftDeleteForUi(string request)
        {
            base.CheckRole("OperationClaim.SoftDelete");
            return base.SoftDeleteForUi(request);
        }
        [HttpGet("GetAllSelectedByAuthorityIdUi")]
        public IActionResult GetAllSelectedByAuthorityIdUi(int AuthorityId, string request)
        {
            base.CheckRole("OperationClaim.GetAllSelectedByAuthorityId");
            IPageableFilter<OperationClaim> operationClaimRequest = RequestHelper.GetRequestByGridExpression<OperationClaim>(request);
            var result = new OperationClaim().GetAllSelectedByAuthorityId(AuthorityId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<OperationClaim> successResult = new SuccessGridResult<OperationClaim>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllUnSelectedByAuthorityIdUi")]
        public IActionResult GetAllUnSelectedByAuthorityIdUi(int AuthorityId, string request)
        {
            base.CheckRole("OperationClaim.GetAllUnSelectedByAuthorityId");
            IPageableFilter<OperationClaim> operationClaimRequest = RequestHelper.GetRequestByGridExpression<OperationClaim>(request);
            var result = new OperationClaim().GetAllUnSelectedByAuthorityId(AuthorityId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<OperationClaim> successResult = new SuccessGridResult<OperationClaim>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpPost("SaveSelectedByAuthorityIdUi")]
        public IActionResult SaveSelectedByAuthorityIdUi(int AuthorityId, [FromForm] HandleRequestData data)
        {
            base.CheckRole("OperationClaim.SaveSelectedByAuthorityId");
            dynamic json = JsonConvert.DeserializeObject(data.Request);
            List<int> claimIds = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(json["list"]));
            var result = new OperationClaim().SaveSelectedByAuthorityId(AuthorityId, claimIds);
            if (result.Success)
            {
                return Ok(result);
            }
            var errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllSelectedByUserIdUi")]
        public IActionResult GetAllSelectedByUserId(int UserId, string request)
        {
            base.CheckRole("OperationClaim.GetAllSelectedByUserId");
            IPageableFilter<OperationClaim> operationClaimRequest = RequestHelper.GetRequestByGridExpression<OperationClaim>(request);
            var result = new OperationClaim().GetAllSelectedByUserId(UserId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<OperationClaim> successResult = new SuccessGridResult<OperationClaim>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllUnSelectedByUserIdUi")]
        public IActionResult GetAllUnSelectedByUserIdUi(int UserId, string request)
        {
            base.CheckRole("OperationClaim.GetAllUnSelectedByUserId");
            IPageableFilter<OperationClaim> operationClaimRequest = RequestHelper.GetRequestByGridExpression<OperationClaim>(request);
            var result = new OperationClaim().GetAllUnSelectedByUserId(UserId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<OperationClaim> successResult = new SuccessGridResult<OperationClaim>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpPost("SaveSelectedByUserIdUi")]
        public IActionResult SaveSelectedByUserIdUi(int UserId, [FromForm] HandleRequestData data)
        {
            base.CheckRole("OperationClaim.SaveSelectedByUserId");
            dynamic json = JsonConvert.DeserializeObject(data.Request);
            List<int> claimIds = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(json["list"]));
            var result = new OperationClaim().SaveSelectedByUserId(UserId, claimIds);
            if (result.Success)
            {
                return Ok(result);
            }
            var errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
    }
}
