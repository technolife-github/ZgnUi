using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZgnWebApi.Controllers.Base;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Filters;
using ZgnWebApi.Core.Utilities.Helpers;
using ZgnWebApi.Core.Utilities.IoC;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Entities;
using ZgnWebApi.Integrations.BlueBotics;
using static ZgnWebApi.Integrations.BlueBotics.Models.BlueBotics.Responses;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlueBoticsController : SecureController
    {
        public readonly IBlueBoticsIntegration _blueBoticsIntegration;

        public BlueBoticsController(IBlueBoticsIntegration blueBoticsIntegration)
        {
            _blueBoticsIntegration = blueBoticsIntegration;
            base.AddRole("BlueBotics.GetNodes");
            base.AddRole("BlueBotics.GetAllPickNodesByLoginUser");
            base.AddRole("BlueBotics.GetAllDropNodesByLoginUser");
            base.AddRole("BlueBotics.GetAllChargerNodesByLoginUser");
            base.AddRole("BlueBotics.GetVehicles");
            base.AddRole("BlueBotics.InsertNode");
            base.AddRole("BlueBotics.ExtractNode");
            base.AddRole("BlueBotics.CreateMission");
            base.AddRole("BlueBotics.GetMissionById");
            base.AddRole("BlueBotics.GetAllMissions");
            base.AddRole("BlueBotics.CancelMission");
            base.AddRole("BlueBotics.CancelAllMissions");
        }
        [HttpGet("GetNodes")]
        public IActionResult GetNodes()
        {
            base.CheckRole("BlueBotics.GetNodes");
            var result = new SuccessDataResult<List<Symbol>>(_blueBoticsIntegration.GetNodes().Payload);
            return Ok(result);
        }
        [HttpGet("GetAllPickNodesByLoginUser")]
        public IActionResult GetAllPickNodesByLoginUser()
        {
            base.CheckRole("BlueBotics.GetAllPickNodesByLoginUser");
            var stationIds = new ZgnWebApi.Entities.User().Get(e => e.Id == ServiceTool.GetUserId()).Data.GetStations().Select(e => e.Id);
            stationIds = new Station().GetAll(e => stationIds.Contains(e.Id) && e.Type == "Alma Noktası").Data.Select(e => e.Id);
            var result = new List<Symbol>();
            new StationNode().GetAll(e => stationIds.Contains(e.StationId)).Data.ForEach(e =>
            {
                result.Add(new Symbol() { Name = e.NodeId });
            });
            return Ok(new SuccessDataResult<List<Symbol>>(result));
        }
        [HttpGet("GetAllDropNodesByLoginUser")]
        public IActionResult GetAllDropNodesByLoginUser()
        {
            base.CheckRole("BlueBotics.GetAllDropNodesByLoginUser");
            var stationIds = new ZgnWebApi.Entities.User().Get(e => e.Id == ServiceTool.GetUserId()).Data.GetStations().Select(e => e.Id);
            stationIds = new Station().GetAll(e => stationIds.Contains(e.Id) && e.Type == "Bırakma Noktası").Data.Select(e => e.Id);
            var result = new List<Symbol>();
            new StationNode().GetAll(e => stationIds.Contains(e.StationId)).Data.ForEach(e =>
            {
                result.Add(new Symbol() { Name = e.NodeId });
            });
            return Ok(new SuccessDataResult<List<Symbol>>(result));
        }
        [HttpGet("GetAllChargerNodesByLoginUser")]
        public IActionResult GetAllChargerNodesByLoginUser()
        {
            base.CheckRole("BlueBotics.GetAllChargerNodesByLoginUser");
            var stationIds = new ZgnWebApi.Entities.User().Get(e => e.Id == ServiceTool.GetUserId()).Data.GetStations().Select(e => e.Id);
            stationIds = new Station().GetAll(e => stationIds.Contains(e.Id) && e.Type == "Yerleştirme Noktası").Data.Select(e => e.Id);
            var result = new List<Symbol>();
            new StationNode().GetAll(e => stationIds.Contains(e.StationId)).Data.ForEach(e =>
            {
                result.Add(new Symbol() { Name = e.NodeId });
            });
            return Ok(new SuccessDataResult<List<Symbol>>(result));
        }
        [HttpGet("GetVehicles")]
        public IActionResult GetVehicles()
        {
            base.CheckRole("BlueBotics.GetVehicles");
            var result = _blueBoticsIntegration.GetVehicles();
            return Ok(result);
        }
        [HttpGet("GetAllVehiclesForUi")]
        public IActionResult GetAllVehiclesForUi()
        {
            base.CheckRole("BlueBotics.GetVehicles");
            var result = _blueBoticsIntegration.GetVehicles();
            if (result.RetCode == 0)
            {
                return Ok(new SuccessGridResult<Vehicle>(result.Payload, 0, result.Payload.Count));
            }
            return BadRequest(new ErrorGridResult(result.Error));
        }
        [HttpGet("InsertNode/{vehicle}/{node}")]
        public IActionResult InsertNode(string vehicle, string node)
        {
            base.CheckRole("BlueBotics.InsertNode");
            var result = _blueBoticsIntegration.InsertNode(vehicle, node);
            if (result.RetCode == 0)
            {
                return Ok(new SuccessResult("İşlem Başarılı"));
            }
            return BadRequest(new ErrorResult(result.Error));
        }
        [HttpGet("ExtractNode/{vehicle}")]
        public IActionResult ExtractNode(string vehicle)
        {
            base.CheckRole("BlueBotics.ExtractNode");
            var result = _blueBoticsIntegration.ExtractNode(vehicle);
            if (result.RetCode == 0)
            {
                return Ok(new SuccessResult("İşlem Başarılı"));
            }
            return BadRequest(new ErrorResult(result.Error));
        }
        [HttpGet("CreateMission/{from}/{to}")]
        public IActionResult CreateMission(string from, string to)
        {
            base.CheckRole("BlueBotics.CreateMission");
            var result = _blueBoticsIntegration.AddMission(from, to);
            return Ok(result);
        }
        [HttpGet("GetMissionById/{id}")]
        public IActionResult GetMissionById(string id)
        {
            base.CheckRole("BlueBotics.GetMissionById");
            var result = _blueBoticsIntegration.GetMission(id);
            return Ok(result);
        }
        [HttpGet("GetMissions")]
        public IActionResult GetMissions()
        {
            base.CheckRole("BlueBotics.GetMissions");
            var result = _blueBoticsIntegration.GetMissions();
            return Ok(result);
        }
        [HttpGet("GetAllMissionsUi")]
        public IActionResult GetAllMissionsUi(string? request)
        {
            base.CheckRole("BlueBotics.GetAllMissions");
            IPageableFilter<Mission> misionRequest = RequestHelper.GetRequestByGridExpression<Mission>(request);
            var result = _blueBoticsIntegration.GetMissions();
            if (result.RetCode == 0)
            {
                result.Payload = (misionRequest.Filter != null) ?
                    result.Payload.AsQueryable().Where(misionRequest.Filter).ToList()
                    : result.Payload;
                ISuccessGridResult<Mission> successResult = new SuccessGridResult<Mission>(result.Payload, 0, result.Payload.Count);
                return Ok(successResult);
            }
            IErrorGridResult errorResult = new ErrorGridResult(result.Error);
            return BadRequest(errorResult);
        }
        [HttpGet("MonitorCancelMissionById/{id}")]
        public IActionResult MonitorCancelMissionById(string id)
        {
            base.CheckRole("BlueBotics.CancelMission");
            var result = _blueBoticsIntegration.MonitorCancelMission(id);
            return Ok(result);
        }
        [HttpGet("CancelMissionById/{id}")]
        public IActionResult CancelMissionById(string id)
        {
            base.CheckRole("BlueBotics.CancelMission");
            var result = _blueBoticsIntegration.CancelMission(id);
            return Ok(result);
        }
        [HttpGet("CancelMissions")]
        public IActionResult CancelMissions()
        {
            base.CheckRole("BlueBotics.CancelAllMissions");
            var result = _blueBoticsIntegration.CancelMissions();
            return Ok(result);
        }
    }
}
