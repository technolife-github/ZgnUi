using Microsoft.IdentityModel.Tokens;
#nullable disable
namespace ZgnWebApi.Core.Utilities.Security
{
    public class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        }
    }




}
