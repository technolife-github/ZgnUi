using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZgnWebApi.Core.Utilities.Middlewares
{
    public class ErrorDetails
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public string? Status { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }

}
