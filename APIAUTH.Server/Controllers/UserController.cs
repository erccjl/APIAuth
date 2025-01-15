using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIAUTH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("recoverPassword")]
        public async Task<IActionResult> RecoverPassword(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _userService.RecoverPassword(email, password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _userService.ChangePassword(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
