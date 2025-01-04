using Application.NotificationBackgroundService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationSink _notificationSink;

        public NotificationController(INotificationSink notificationSink)
        {
            _notificationSink = notificationSink;   
        }


        [Authorize]
        [HttpGet("/notify")]
        public async Task<IActionResult> Notify(string user, string message)
        {
            await _notificationSink.PushAsync(new(user, message));
            return Ok();
        }
    }
}
