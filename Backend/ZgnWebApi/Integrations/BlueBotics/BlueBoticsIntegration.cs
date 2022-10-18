using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using static ZgnWebApi.Integrations.BlueBotics.Models.BlueBotics;
using static ZgnWebApi.Integrations.BlueBotics.Models.BlueBotics.Responses;
using System.Linq;
using System.Diagnostics;
#nullable disable
namespace ZgnWebApi.Integrations.BlueBotics
{
    public interface IBlueBoticsIntegration
    {
        Response<AddMisson> AddMission(string from, string to);
        Response<CancelMission> CancelMission(string missionId);
        Response<ListCancelMission> CancelMissions();
        Response ExtractNode(string vehicle);
        int GetDelay();
        Response<Mission> GetMission(string missionId);
        Response<List<Mission>> GetMissions();
        Response<List<Symbol>> GetNodes();
        Response<List<Vehicle>> GetVehicles();
        Response InsertNode(string vehicle, string node);
    }
    public class BlueBoticsIntegration : IBlueBoticsIntegration
    {
        public BlueBoticsConfig Config;
        private readonly IConfiguration Configuration;
        private HttpClient HttpClient;
        public BlueBoticsIntegration(IConfiguration configuration)
        {
            this.Configuration = configuration;
            Config = Configuration.GetSection("BlueBotics").Get<BlueBoticsConfig>();
        }
        public Response<Login> Login()
        {
            //make http client get
            HttpClient = new HttpClient();
            var response = HttpClient.GetAsync($"{Config.ApiUrl}/wms/monitor/session/login?username={Config.UserName}&pwd={Config.Password}").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Response<Login>>(resultString);
                if (result.RetCode == 0)
                    AppData.Login = result.Payload;
                return result;
            }
            throw new BlueBoticsException("User login failed");
        }
        public Response InsertNode(string vehicle, string node)
        {
            this.Login();
            var sendObj = new { command = new { name = "insert", args = new { nodeId = node } } };
            var content = new StringContent(JsonConvert.SerializeObject(sendObj), Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync($"{Config.ApiUrl}/wms/rest/vehicles/{vehicle}/command?sessiontoken={AppData.Login.SessionToken}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Response>(resultString);
                return result;
            }
            throw new BlueBoticsException("Insert node failed");
        }
        public Response ExtractNode(string vehicle)
        {
            this.Login();
            var sendObj = new { command = new { name = "extract", args = new {} } };
            var content = new StringContent(JsonConvert.SerializeObject(sendObj), Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync($"{Config.ApiUrl}/wms/rest/vehicles/{vehicle}/command?sessiontoken={AppData.Login.SessionToken}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Response>(resultString);
                return result;
            }
            throw new BlueBoticsException("Insert node failed");
        }
        public Response<List<Symbol>> GetNodes()
        {
            this.Login();
            Response<List<Symbol>> symbolResult = new();

            //make http client get
            Config.Levels.ForEach(level =>
            {
                HttpClient = new HttpClient();
                Debug.WriteLine($"{Config.ApiUrl}/wms/rest/maps/level/{level}/data?sessiontoken={AppData.Login.SessionToken}");
                var response = HttpClient.GetAsync($"{Config.ApiUrl}/wms/rest/maps/level/{level}/data?sessiontoken={AppData.Login.SessionToken}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var resultString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Response<DataResponse<List<FilterInfo<Map>>>>>(resultString);
                    result.Payload.Data.ForEach(data =>
                    {
                        data.Data.Layers.ForEach(layer =>
                        {
                            if (symbolResult.Payload == null)
                                symbolResult.Payload = layer.Symbols;
                            else
                                layer.Symbols.ForEach(symbol =>
                                {
                                    symbolResult.Payload.Add(symbol);
                                });
                        });
                    });
                }
            });
            return symbolResult;
        }
        public Response<List<Vehicle>> GetVehicles()
        {
            this.Login();
            HttpClient = new HttpClient();
            var response = HttpClient.GetAsync($"{Config.ApiUrl}/wms/rest/vehicles?sessiontoken={AppData.Login.SessionToken}&dataorderby=[[\"name\",\"asc\"]]").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Response<VehiclesData>>(resultString);
                return new Response<List<Vehicle>>()
                {
                    Error = result.Error,
                    Payload = result.Payload.Vehicles,
                    RetCode = result.RetCode
                };
            }
            throw new BlueBoticsException("Get vehicles failed");
        }
        public Response<AddMisson> AddMission(string from, string to)
        {
            this.Login();
            var parameter = JsonConvert.DeserializeObject(@"{
	            ""value"": {
		        ""payload"": ""Default Payload""
	            },
	            ""desc"": ""Mission extension"",
	            ""type"": ""org.json.JSONObject"",
                    ""name"": ""parameters""
    	        }");
            var sendObj = new
            {
                missionrequest = new
                {
                    requestor = "insert",
                    missiontype = 7,
                    fromnode = from,
                    tonode = to,
                    cardinality = 1,
                    priority = 2,
                    parameters = parameter
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(sendObj), Encoding.UTF8, "application/json");
            var response = HttpClient.PostAsync($"{Config.ApiUrl}/wms/rest/missions?sessiontoken={AppData.Login.SessionToken}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Response<AddMisson>>(resultString);
                return result;
            }
            throw new BlueBoticsException("Insert node failed");
        }
        public Response<Mission> GetMission(string missionId)
        {
            this.Login();
            HttpClient = new HttpClient();
            var response = HttpClient.GetAsync($"{Config.ApiUrl}/wms/rest/missions/{missionId}?sessiontoken={AppData.Login.SessionToken}").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Response<MissionData>>(resultString);
                return new Response<Mission>()
                {
                    Error = result.Error,
                    Payload = result.Payload.Missions.FirstOrDefault(),
                    RetCode = result.RetCode
                };
            }
            throw new BlueBoticsException("Get mission failed");
        }
        public Response<List<Mission>> GetMissions()
        {
            this.Login();
            HttpClient = new HttpClient();
            var response = HttpClient.GetAsync($"{Config.ApiUrl}/wms/rest/missions?sessiontoken={AppData.Login.SessionToken}&dataorderby=[[\"createdat\",\"desc\"]]").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Response<MissionData>>(resultString);
                return new Response<List<Mission>>()
                {
                    Error = result.Error,
                    Payload = result.Payload.Missions,
                    RetCode = result.RetCode
                };
            }
            throw new BlueBoticsException("Get mission failed");
        }
        public Response<CancelMission> CancelMission(string missionId)
        {
            this.Login();
            HttpClient = new HttpClient();
            var response = HttpClient.DeleteAsync($"{Config.ApiUrl}/wms/rest/missions/{missionId}?sessiontoken={AppData.Login.SessionToken}").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Response<CancelMission>>(resultString);
            }
            throw new BlueBoticsException("Get mission failed");
        }
        public Response<ListCancelMission> CancelMissions()
        {
            this.Login();
            HttpClient = new HttpClient();
            var response = HttpClient.DeleteAsync($"{Config.ApiUrl}/wms/rest/missions?sessiontoken={AppData.Login.SessionToken}&dataorderby=[[\"createdat\",\"desc\"]]").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Response<ListCancelMission>>(resultString);
            }
            throw new BlueBoticsException("Get mission failed");
        }
        public int GetDelay()
        {
            return Config.Delay;
        }
        public class BlueBoticsConfig
        {
            public string ApiUrl { get; set; }
            public List<int> Levels { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int Delay { get; set; }
        }
        public class BlueBoticsException : Exception
        {
            public BlueBoticsException() : base("Unknown Error!")
            {

            }
            public BlueBoticsException(string message) : base(message)
            {

            }
        }
    }
}
