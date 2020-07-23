using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Models.Reporting.Record;
using todo_list_backend.Services;
using todo_list_backend.Services.Project;

namespace todo_list_backend.Models.Reporting.Dto
{
    public class TaskAddedReport : IReportable
    {
        public int ProjectId { get; set; }
        public int AddedByUserId { get; set; }
        public int ProjectTaskId { get; set; }

        public CreateNotificationDto Compose(IServiceProvider services)
        {
            var userService = services.GetService<IUserService>();
            var projectTaskService = services.GetService<IProjectTaskService>();

            var addedBy = userService.FindById(AddedByUserId);
            var taskDto = projectTaskService.Find(ProjectTaskId);

            var nullResult = new CreateNotificationDto { isLink = false, Link="", Subject="", Message="" };

            return userService.FindById(AddedByUserId).Get(addedBy =>
            {
                return projectTaskService.Find(ProjectTaskId).Get(taskDto =>
                {
                    var assignmentWording = addedBy.DisplayName == taskDto.AssignedTo.DisplayName 
                        ? "self-assigned" 
                        : String.Format("assigned {0}", taskDto.AssignedTo.DisplayName);

                    var message = String.Format("{0} {1} a new task: {2}",
                        addedBy.DisplayName,
                        assignmentWording,
                        taskDto.Label
                    );

                    return new CreateNotificationDto
                    {
                        isLink = false,
                        Link = "",
                        Subject = "Task added",
                        Message = message
                    };
                }, () => nullResult);
            }, () => nullResult);
        }

        public ActivityLogItem ToRecord()
        {
            return new ActivityLogItem
            {
                ProjectId = ProjectId,
                UserId = AddedByUserId,
                Category = Constants.ActivityLogCategories.TASK_ADDED,
                Values = new Dictionary<string, string> { { "projectTaskId", ProjectTaskId.ToString() } }
            };
        }
    }
}
