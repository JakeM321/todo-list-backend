using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_list_backend.Services;

namespace todo_list_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private INotificationProviderService _notificationProviderService;

        public NotificationsController(INotificationProviderService notificationProviderService)
        {
            _notificationProviderService = notificationProviderService;
        }

        [HttpPost]
        [Authorize(Policy = "VerifyNotificationOwnership")]
        [Route("mark-as-seen")]
        public IActionResult MarkAsSeen([FromBody] int[] notificationIds)
        {
            _notificationProviderService.MarkAsSeen(notificationIds);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("hello")]
        public string Hello()
        {
            return "Hi";
        }
    }
}
