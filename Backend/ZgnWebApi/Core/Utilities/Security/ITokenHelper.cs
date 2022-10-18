using System.IdentityModel.Tokens.Jwt;
using ZgnWebApi.Entities;
#nullable disable
namespace ZgnWebApi.Core.Utilities.Security
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
        JwtSecurityToken ReadToken(string token);
    }




}
