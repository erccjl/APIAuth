using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Interfaces;
using APIAUTH.Domain.Entities;
using APIAUTH.Infrastructure.Services;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace APIAUTH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly IUserService _userService;

        public UserController(IUserService userService, NotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        [HttpPost("recoverPassword")]
        public async Task<IActionResult> RecoverPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _userService.RecoverPassword(email);
                await _notificationService.NotifyUser("1", "");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(UserPasswordDto userPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {               
                return Ok(await _userService.ChangePassword(userPasswordDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
