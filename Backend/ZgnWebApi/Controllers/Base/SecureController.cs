using Microsoft.AspNetCore.Mvc;
using ZgnWebApi.Core.Constants;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.Middlewares;
using ZgnWebApi.Entities;

namespace ZgnWebApi.Controllers.Base
{
    public abstract class SecureController : ControllerBase
    {

        [ApiExplorerSettings(IgnoreApi = true)]
        public void CheckRole(string roles)
        {
            var roleClaims = User.ClaimRoles();
            foreach (var role in $"{roles},Supervisor".Split(','))
            {
                if (role != "Supervisor") new OperationClaim().CheckAndAddByName(role);
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new AuthenticationException(Messages.AuthorizationDenied);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public void AddRole(string role)
        {
            if (role != "Supervisor" && role != "supervisor") new OperationClaim().CheckAndAddByName(role);
        }
    }
}
