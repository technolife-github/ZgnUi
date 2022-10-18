using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Core.Utilities.Security;

namespace ZgnWebApi.Entities
{
    /***** DTOS *****/
    public class UserForLoginDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public IDataResult<AccessToken> Login()
        {
            var userToCheck = new User().Get(u => u.UserName == UserName && u.DeletedAt == null).Data;
            if (userToCheck == null)
                return new ErrorDataResult<AccessToken>("userNotFound");
            if (!userToCheck.Status)
                return new ErrorDataResult<AccessToken>("userNotActive");
            if (userToCheck.Banned)
                return new ErrorDataResult<AccessToken>($"youAreBanned->{userToCheck.BannedMsg}");

            if (!HashingHelper.VerifyPasswordHash(Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<AccessToken>("passwordError");
            }
            return userToCheck.CreateAccessToken();
        }
    }
}
