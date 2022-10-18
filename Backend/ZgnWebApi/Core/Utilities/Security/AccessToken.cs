#nullable disable
namespace ZgnWebApi.Core.Utilities.Security
{
    public class AccessToken
    {
        public string Token { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public DateTime Expiration { get; set; }
    }




}
