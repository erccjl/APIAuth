using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIAUTH.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var tokens = await _authenticationService.AuthenticateUserAsync(request.UserName, request.Password);
                return Ok(new { idToken = tokens.idToken, accessToken = tokens.accessToken });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials.");
            }
        }

        [HttpPost("loginSSO")]
        public async Task<IActionResult> LoginSSO(string authorizationCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var tokens = await _authenticationService.AuthenticateWithGoogleAsync(authorizationCode);
                return Ok(new { idToken = tokens.idToken, accessToken = tokens.accessToken });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

    }
}