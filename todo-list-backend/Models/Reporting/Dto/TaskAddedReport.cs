﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Models.Project.Dto.Activity;
using todo_list_backend.Models.Reporting.Record;
using todo_list_backend.Services;
using todo_list_backend.Services.Project;

namespace todo_list_backend.Models.Reporting.Dto
{
    public class TaskAddedReport : IReportable, IComposibleProjectActivity
    {
        public int ProjectId { get; set; }
        public int AddedByUserId { get; set; }
        public int ProjectTaskId { get; set; }

        public TaskAddedReport() { }

        public TaskAddedReport(ActivityLogItem item)
        {
            ProjectId = item.ProjectId;
            AddedByUserId = item.UserId;
            ProjectTaskId = item.Values.ContainsKey("projectTaskId") 
                ? Convert.ToInt32(item.Values["projectTaskId"]) 
                : 0;
        }

        private string GetMessage(IServiceProvider services, bool addressingAssignee)
        {
            var userService = services.GetService<IUserService>();
            var projectTaskService = services.GetService<IProjectTaskService>();

            var addedBy = userService.FindById(AddedByUserId);
            var taskDto = projectTaskService.Find(ProjectTaskId);

            return userService.FindById(AddedByUserId).Get(addedBy =>
            {
                return projectTaskService.Find(ProjectTaskId).Get(taskDto =>
                {
                    var assignmentWording = addressingAssignee 
                        ? "assigned you"
                        : addedBy.DisplayName == taskDto.AssignedTo.DisplayName
                            ? "self-assigned"
                            : String.Format("assigned {0}", taskDto.AssignedTo.DisplayName);

                    var message = String.Format("{0} {1} a new task: {2}",
                        addedBy.DisplayName,
                        assignmentWording,
                        taskDto.Label
                    );

                    return message;
                }, () => "");
            }, () => "");
        }

        public CreateNotificationDto Compose(IServiceProvider services)
        {
            return new CreateNotificationDto
            {
                isLink = false,
                Link = "",
                Subject = String.Format("Task added to project"),
                Message = GetMessage(services, true)
            };
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

        public ProjectActivityDto ToProjectActivity(IServiceProvider services)
        {
            return new ProjectActivityDto
            {
                Description = GetMessage(services, false)
            };
        }
    }
}
