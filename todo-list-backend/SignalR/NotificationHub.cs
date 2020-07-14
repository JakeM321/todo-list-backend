using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Models.User;
using todo_list_backend.Services;

namespace todo_list_backend.SignalR
{
    public class NotificationHub : Hub
    {
        private INotificationHubManager _manager;
        public NotificationHub(INotificationHubManager manager)
        {
            _manager = manager;
        }

        public override Task OnConnectedAsync()
        {
            var user = (UserDto)this.Context.GetHttpContext().Items["user"];
            _manager.RegisterConnection(user.Id, Context.ConnectionId);

            return base.OnConnectedAsync();
        }
    }
}
