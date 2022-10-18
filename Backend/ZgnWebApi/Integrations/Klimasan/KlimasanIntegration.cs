using Newtonsoft.Json;
using ZgnWebApi.Integrations.Klimasan.Models;
#nullable disable
namespace ZgnWebApi.Integrations.Klimasan
{
    public interface IKlimasanIntegration
    {
        DataResponse<List<GroupDto>> GetGroups();
        DataResponse<List<InventoryDto>> GetInventories(int productId);
        DataResponse<List<ProductDto>> GetProducts(string groupName);
    }
    public class KlimasanIntegration : IKlimasanIntegration
    {

        public KlimasanConfig Config;
        private readonly IConfiguration Configuration;
        private HttpClient HttpClient;
        public KlimasanIntegration(IConfiguration configuration)
        {
            this.Configuration = configuration;
            Config = Configuration.GetSection("Klimasan").Get<KlimasanConfig>();
        }
        public DataResponse<List<GroupDto>> GetGroups()
        {
            HttpClient = new HttpClient();
            var response = HttpClient.GetAsync($"{Config.ApiUrl}/api/Default/GroupList").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<DataResponse<List<GroupDto>>>(resultString);
            }
            throw new KlimasanException("GroupsNotAvailable");
        }

        public DataResponse<List<ProductDto>> GetProducts(string groupName)
        {
            HttpClient = new HttpClient();
            var response = HttpClient.GetAsync($"{Config.ApiUrl}/api/Default/GetStocks/{groupName}").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<DataResponse<List<ProductDto>>>(resultString);
            }
            throw new KlimasanException();
        }

        public DataResponse<List<InventoryDto>> GetInventories(int productId)
        {
            HttpClient = new HttpClient();
            var response = HttpClient.GetAsync($"{Config.ApiUrl}/api/Default/GetInventory/{productId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var resultString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<DataResponse<List<InventoryDto>>>(resultString);
            }
            throw new KlimasanException();
        }

        public class KlimasanConfig
        {
            public string ApiUrl { get; set; }
        }
        public class KlimasanException : Exception
        {
            public KlimasanException() : base("Unknown Error!")
            {

            }
            public KlimasanException(string message) : base(message)
            {

            }
        }
    }
}
