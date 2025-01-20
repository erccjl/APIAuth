using APIAUTH.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIAUTH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationsController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> Broadcast([FromBody] string message)
        {
            await _notificationService.NotifyAll(message);
            return Ok("Notification sent to all users.");
        }

        [HttpPost("user/{userId}")]
        public async Task<IActionResult> NotifyUser(string userId, [FromBody] string message)
        {
            await _notificationService.NotifyUser(userId, message);
            return Ok($"Notification sent to user {userId}.");
        }
    }
}
