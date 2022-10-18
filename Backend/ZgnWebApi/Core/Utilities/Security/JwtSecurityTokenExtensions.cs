using System.IdentityModel.Tokens.Jwt;
#nullable disable
namespace ZgnWebApi.Core.Utilities.Security
{
    public static class JwtSecurityTokenExtensions
    {
        public static bool HasRole(this JwtSecurityToken token, string role)
        {
            if (token == null)
                return false;
            return token.Claims.ToList().FindAll(i => role.Split(',').Contains(i.Value)).Count > 0;
        }
    }




}
