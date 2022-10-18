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
using ZgnWebApi.Integrations.Klimasan;
using ZgnWebApi.Integrations.Klimasan.Models;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlimasanController : SecureController
    {
        private readonly IKlimasanIntegration _klimasanIntegration;

        public KlimasanController(IKlimasanIntegration klimasanIntegration)
        {
            _klimasanIntegration = klimasanIntegration;
            base.AddRole("Klimasan.GetGroups");
            base.AddRole("Klimasan.GetAllGroupsByLoginUser");
            base.AddRole("Klimasan.GetProductsByGroupCode");
            base.AddRole("Klimasan.GetInventoriesByProductId");
        }
        [HttpGet("GetGroups")]
        public IActionResult GetGroups()
        {
            base.CheckRole("Klimasan.GetGroups");
            return Ok(_klimasanIntegration.GetGroups());
        }
        [HttpGet("GetAllGroupsByLoginUser")]
        public IActionResult GetAllGroupsByLoginUser()
        {
            base.CheckRole("Klimasan.GetAllGroupsByLoginUser");
            var stationIds = new User().Get(e => e.Id == ServiceTool.GetUserId()).Data.GetStations().Select(e => e.Id);
            stationIds = new Station().GetAll(e => stationIds.Contains(e.Id) && e.Type == "Alma Noktası").Data.Select(e => e.Id);
            var result = new List<GroupDto>();
            new StationGroupCode().GetAll(e => stationIds.Contains(e.StationId)).Data.ForEach(e =>
            {
                result.Add(new GroupDto() { GroupCode = e.GroupCode });
            });
            return Ok(new SuccessDataResult<List<GroupDto>>(result));
        }
        [HttpGet("GetProductsByGroupCode/{groupCode}")]
        public IActionResult GetProductsByGroupCode(string groupCode)
        {
            base.CheckRole("Klimasan.GetProductsByGroupCode");
            return Ok(_klimasanIntegration.GetProducts(groupCode));
        }
        [HttpGet("GetInventoriesByProductId/{productId}")]
        public IActionResult GetInventoriesByProductId(int productId)
        {
            base.CheckRole("Klimasan.GetInventoriesByProductId");
            return Ok(_klimasanIntegration.GetInventories(productId));
        }
    }
}
