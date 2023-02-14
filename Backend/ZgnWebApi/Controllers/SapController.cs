using Microsoft.AspNetCore.Mvc;
using ZgnWebApi.Controllers.Base;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Integrations.Sap;
using ZgnWebApi.Integrations.Sap.Models;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SapController : SecureController
    {
        private readonly ISapIntegration _sapIntegration;

        public SapController(ISapIntegration sapIntegration)
        {
            _sapIntegration = sapIntegration;
            base.AddRole("Sap.GetGroups");
            base.AddRole("Sap.GetAllByGroupName");
        }
        [HttpGet("GetGroups")]
        public IActionResult GetGroups()
        {
            base.CheckRole("Sap.GetGroups");
            return Ok(new SuccessDataResult<List<SapGroupDto>>(_sapIntegration.Config.Groups, "Sap groups listed"));
        }
        [HttpGet("GetAllByGroupName/{groupName}")]
        public IActionResult GetAllByGroupName(string groupName)
        {
            base.CheckRole("Sap.GetAllByGroupName");
            return Ok(new SuccessDataResult<List<SapData>>(_sapIntegration.GetData(groupName).ITAB, $"Get listed by {groupName}."));
        }
        
    }
}
