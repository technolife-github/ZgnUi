using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
#nullable disable
namespace ZgnWebApi.Core.Utilities.Security
{
    public static class ClaimExtensions
    {
        public static void AddUserName(this ICollection<Claim> claims, string username)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, username));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }
        public static void AddCustomerCode(this ICollection<Claim> claims, string customerCode)
        {
            claims.Add(new Claim(ClaimTypes.Locality, customerCode is null ? "" : customerCode));
        }
        public static void AddUserType(this ICollection<Claim> claims, string userType)
        {
            claims.Add(new Claim(ClaimTypes.IsPersistent, userType is null ? "" : userType));
        }
        public static void AddDealerId(this ICollection<Claim> claims, string dealerId)
        {
            claims.Add(new Claim(ClaimTypes.GroupSid, dealerId is null ? "" : dealerId));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }




}
