using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using ZgnWebApi.Controllers.Base;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Helpers;
using ZgnWebApi.Core.Utilities.IoC;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Entities;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : PageableController<Station>
    {
        public StationsController()
        {
            base.AddRole("Station.GetAll");
            base.AddRole("Station.Get");
            base.AddRole("Station.Add");
            base.AddRole("Station.Update");
            base.AddRole("Station.Delete");
            base.AddRole("Station.SoftDelete");
        }
        public override IActionResult GetAll()
        {
            base.CheckRole("Station.GetAll");
            return base.GetAll();
        }
        public override IActionResult GetById(int id)
        {
            base.CheckRole("Station.Get");
            return base.GetById(id);
        }
        public override IActionResult Add(Station entity)
        {
            base.CheckRole("Station.Add");
            return base.Add(entity);
        }
        public override IActionResult Update(Station entity)
        {
            base.CheckRole("Station.Update");
            return base.Update(entity);
        }
        public override IActionResult Delete(Station entity)
        {
            base.CheckRole("Station.Delete");
            return base.Delete(entity);
        }
        public override IActionResult GetAllForUi(string? request)
        {
            base.CheckRole("Station.GetAll");
            return base.GetAllForUi(request);
        }
        public override IActionResult GetForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Station.Get");
            return base.GetForUi(data);
        }
        public override IActionResult AddForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Station.Add");
            return base.AddForUi(data);
        }
        public override IActionResult UpdateForUi([FromForm] HandleRequestData data)
        {
            base.CheckRole("Station.Update");
            return base.UpdateForUi(data);
        }
        public override IActionResult DeleteForUi(string request)
        {
            base.CheckRole("Station.Delete");
            return base.DeleteForUi(request);
        }
        public override IActionResult SoftDeleteForUi(string request)
        {
            base.CheckRole("Station.SoftDelete");
            return base.SoftDeleteForUi(request);
        }
        [HttpGet("GetAllSelectedByAuthorityIdUi")]
        public IActionResult GetAllSelectedByAuthorityIdUi(int AuthorityId, string request)
        {
            base.CheckRole("Station.GetAllSelectedByAuthorityId");
            IPageableFilter<Station> operationClaimRequest = RequestHelper.GetRequestByGridExpression<Station>(request);
            var result = new Station().GetAllSelectedByAuthorityId(AuthorityId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Station> successResult = new SuccessGridResult<Station>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllUnSelectedByAuthorityIdUi")]
        public IActionResult GetAllUnSelectedByAuthorityIdUi(int AuthorityId, string request)
        {
            base.CheckRole("Station.GetAllUnSelectedByAuthorityId");
            IPageableFilter<Station> operationClaimRequest = RequestHelper.GetRequestByGridExpression<Station>(request);
            var result = new Station().GetAllUnSelectedByAuthorityId(AuthorityId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Station> successResult = new SuccessGridResult<Station>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpPost("SaveSelectedByAuthorityIdUi")]
        public IActionResult SaveSelectedByAuthorityIdUi(int AuthorityId, [FromForm] HandleRequestData data)
        {
            base.CheckRole("Station.SaveSelectedByAuthorityId");
            dynamic json = JsonConvert.DeserializeObject(data.Request);
            List<int> claimIds = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(json["list"]));
            var result = new Station().SaveSelectedByAuthorityId(AuthorityId, claimIds);
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
            base.CheckRole("Station.GetAllSelectedByUserIdUi");
            IPageableFilter<Station> operationClaimRequest = RequestHelper.GetRequestByGridExpression<Station>(request);
            var result = new Station().GetAllSelectedByUserId(UserId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Station> successResult = new SuccessGridResult<Station>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllUnSelectedByUserIdUi")]
        public IActionResult GetAllUnSelectedByUserIdUi(int UserId, string request)
        {
            base.CheckRole("Station.GetAllUnSelectedByUserId");
            IPageableFilter<Station> operationClaimRequest = RequestHelper.GetRequestByGridExpression<Station>(request);
            var result = new Station().GetAllUnSelectedByUserId(UserId, operationClaimRequest);
            if (result.Success)
            {
                var data = result.Data.Data;
                ISuccessGridResult<Station> successResult = new SuccessGridResult<Station>(data, operationClaimRequest.Pagination.Limit, result.Data.TotalCount);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpPost("SaveSelectedByUserIdUi")]
        public IActionResult SaveSelectedByUserIdUi(int UserId, [FromForm] HandleRequestData data)
        {
            base.CheckRole("Station.SaveSelectedByUserId");
            dynamic json = JsonConvert.DeserializeObject(data.Request);
            List<int> claimIds = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(json["list"]));
            var result = new Station().SaveSelectedByUserId(UserId, claimIds);
            if (result.Success)
            {
                return Ok(result);
            }
            var errorResult = new ErrorGridResult(result.Message);
            return BadRequest(errorResult);
        }
        [HttpGet("GetAllByLoginUser")]
        public IActionResult GetAllByLoginUser()
        {
            base.CheckRole("Station.GetAllByLoginUser");
            var result = new Station().GetAllByUserId(ServiceTool.GetUserId());
            return Ok(result);
        }


    }
}
