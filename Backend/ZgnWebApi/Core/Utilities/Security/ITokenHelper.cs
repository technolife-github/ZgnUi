using System.IdentityModel.Tokens.Jwt;
using ZgnWebApi.BackgroundWorkers;
using ZgnWebApi.Entities;
#nullable disable
namespace ZgnWebApi.Core.Utilities.Security
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
        JwtSecurityToken ReadToken(string token);
        AccessToken CreateWebSocketToken(ZgnWebSocketUser user);
    }




}
