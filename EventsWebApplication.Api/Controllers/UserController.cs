using EventsWebApplication.Application.Services;
using EventsWebApplication.Core.Contracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _service;
        private readonly IValidator<RegisterUserRequest> _validator;
        private readonly IValidator<LoginUserRequest> _loginValidator;


        public UserController(IUserService service, IValidator<RegisterUserRequest> validator, IValidator<LoginUserRequest> loginValidator)
        {

            _service = service;
            _validator = validator;
            _loginValidator = loginValidator;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            ValidationResult result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            await _service.Register(request.userName, request.email, request.password);

            return Ok(request);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            try
            {
                ValidationResult result = await _loginValidator.ValidateAsync(request);

                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                var token = await _service.Login(request.email, request.password);

                HttpContext.Response.Cookies.Append("tasty-cookies", token);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
    }
}
