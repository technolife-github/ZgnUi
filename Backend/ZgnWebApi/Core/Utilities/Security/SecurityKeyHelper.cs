using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
#nullable disable
namespace ZgnWebApi.Core.Utilities.Security
{
    public class SecurityKeyHelper
    {
        private readonly IConfiguration Configuration;
        public SecurityKeyHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private static Random random = new Random();

        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
        public string CreateToken(string key)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(Configuration.GetSection("TokenOptions").Get<TokenOptions>().SecurityKey);
            byte[] messageBytes = encoding.GetBytes(key);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashMessage);
            }
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnoprstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }




}
