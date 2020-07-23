using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Models.Project.Dto.Activity;
using todo_list_backend.Models.Reporting.Record;
using todo_list_backend.Services;
using todo_list_backend.Services.Project;

namespace todo_list_backend.Models.Reporting.Dto
{
    public class MemberAddedReport : IReportable, IComposibleProjectActivity
    {
        public int ProjectId { get; set; }
        public int AddedByUserId { get; set; }
        public int MemberUserId { get; set; }

        public MemberAddedReport() { }

        public MemberAddedReport(ActivityLogItem item)
        {
            ProjectId = item.ProjectId;
            AddedByUserId = item.UserId;
            MemberUserId = item.Values.ContainsKey("addedMemberUserId")
                ? Convert.ToInt32(item.Values["addedMemberUserId"])
                : 0;
        }

        public CreateNotificationDto Compose(IServiceProvider services)
        {
            var userService = services.GetService<IUserService>();
            var projectService = services.GetService<IProjectService>();

            var nullResult = new CreateNotificationDto { };

            return userService.FindById(AddedByUserId).Get(
                user => projectService.GetInfo(MemberUserId, ProjectId).Get(
                    info => new CreateNotificationDto
                    {
                        isLink = false,
                        Link = "",
                        Subject = "New project invitation",
                        Message = String.Format("{0} added you to the project: {1}", user.DisplayName, info.Title)
                    }, () => nullResult
                ), () => nullResult
            );
        }

        public ProjectActivityDto ToProjectActivity(IServiceProvider services)
        {
            var userService = services.GetService<IUserService>();
            var nullResult = new ProjectActivityDto { };

            return userService.FindById(AddedByUserId).Get(
                addedBy => userService.FindById(MemberUserId).Get(
                    newMember => new ProjectActivityDto
                    {
                        Description = String.Format("{0} added {1} to the project", addedBy.DisplayName, newMember.DisplayName)
                    },
                    () => nullResult),
                () => nullResult
            );
        }

        public ActivityLogItem ToRecord()
        {
            return new ActivityLogItem
            {
                Category = Constants.ActivityLogCategories.MEMBER_ADDED,
                ProjectId = ProjectId,
                UserId = AddedByUserId,
                Values = new Dictionary<string, string> { { "addedMemberUserId", MemberUserId.ToString() } }
            };
        }
    }
}
