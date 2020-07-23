using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Models.Reporting.Dto;
using todo_list_backend.Repositories;

namespace todo_list_backend.Services.Reporting
{
    public class ReportingService : IReportingService
    {
        private IServiceProvider _serviceProvider;
        private INotificationSenderService _notificationSenderService;
        private IActivityLogService _activityLogService;

        public ReportingService(
            IServiceProvider serviceProvider,
            INotificationSenderService notificationSenderService,
            IActivityLogService activityLogService
        )
        {
            _serviceProvider = serviceProvider;
            _notificationSenderService = notificationSenderService;
            _activityLogService = activityLogService;
        }

        public void Report(IReportable reportable, int[] affectedUserIds)
        {
            _activityLogService.LogActivity(reportable);

            var notification = reportable.Compose(_serviceProvider);

            foreach (int userId in affectedUserIds) {
                _notificationSenderService.SendNotificationAsync(new CreateUserNotificationDto
                {
                    isLink = notification.isLink,
                    Link = notification.Link,
                    Message = notification.Message,
                    Subject = notification.Subject,
                    UserId = userId
                });
            }
        }
    }
}
