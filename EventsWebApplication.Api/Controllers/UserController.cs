using EventsWebApplication.Application.Users.Login;
using EventsWebApplication.Application.Users.RefreshToken;
using EventsWebApplication.Application.Users.Registration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EventsWebApplication.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRegistrationUseCase _userRegistrationUseCase;
        private readonly UserLoginUseCase _userLoginUseCase;
        private readonly RefreshTokenUseCase _refreshTokenUseCase;

        public UserController(UserRegistrationUseCase userRegistrationUseCase, UserLoginUseCase userLoginUseCase, RefreshTokenUseCase refreshTokenUseCase)
        {
            _userRegistrationUseCase = userRegistrationUseCase;
            _userLoginUseCase = userLoginUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            UserRegistrationResponse response = await _userRegistrationUseCase.Register(request);
            return Ok(response);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            UserLoginResponse response = await _userLoginUseCase.Login(request);
            HttpContext.Response.Cookies.Append("tasty-cookies", response.JwtToken);

            return Ok(response);

        }

        [HttpPost("refresh /{refreshToken}")]

        public async Task<IActionResult> Refresh([FromRoute] string refreshToken )
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out var jwtToken))
            {
                return Unauthorized();
            }
            
            RefreshTokenResponse response = await _refreshTokenUseCase.Refresh(new RefreshTokenRequest { JwtToken = jwtToken, RefreshToken = refreshToken });
            HttpContext.Response.Cookies.Append("tasty-cookies", response.JwtToken);
            return Ok(response);

        }


    }
}
