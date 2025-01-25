using APIAUTH.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace APIAUTH.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAll(string message)
        {
            var notification = new
            {
                Title = "Nueva Notificación",
                Message = message,
                Timestamp = DateTime.UtcNow
            };
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }

        public async Task NotifyUser(string userId, string message)
        {
            var notification = new
            {
                Title = "Nueva Notificación",
                Message = message,
                Timestamp = DateTime.UtcNow
            };
      
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
