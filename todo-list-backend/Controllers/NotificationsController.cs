using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_list_backend.Services;
using todo_list_backend.Utils;

namespace todo_list_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ApiController
    {
        private INotificationProviderService _notificationProviderService;

        public NotificationsController(INotificationProviderService notificationProviderService)
        {
            _notificationProviderService = notificationProviderService;
        }

        [HttpPost]
        [Authorize(Policy = "NotificationsBelongToUser")]
        [Route("mark-as-seen")]
        public IActionResult MarkAsSeen([FromBody] int[] notificationIds)
        {
            _notificationProviderService.MarkAsSeen(notificationIds);
            return Ok();
        }

        [HttpGet]
        [Route("get-notifications")]
        public IActionResult Notifications([FromQuery] int skip, [FromQuery] int take)
        {
            return withUser(Request, user => {
                var result = _notificationProviderService.GetUserNotifications(user.Id, skip, take);
                return new JsonResult(result);
            });
        }
    }
}
