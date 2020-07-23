using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;

namespace todo_list_backend.Models.Reporting.Dto
{
    public interface IComposibleNotification
    {
        CreateNotificationDto Compose(IServiceProvider services);
    }
}
