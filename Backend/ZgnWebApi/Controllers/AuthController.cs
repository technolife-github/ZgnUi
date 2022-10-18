using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZgnWebApi.Entities;

namespace ZgnWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto loginDto)
        {
            var login = loginDto.Login();
            if (!login.Success)
                return BadRequest(login);
            return Ok(login.Data);
        }

        [HttpGet("isAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            var username = User?.Identity?.Name;
            return Ok(username != null);
        }

    }
}
