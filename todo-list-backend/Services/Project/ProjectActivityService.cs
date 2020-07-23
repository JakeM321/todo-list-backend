using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Activity;
using todo_list_backend.Models.Reporting.Dto;
using todo_list_backend.Models.Reporting.Record;
using todo_list_backend.Services.Reporting;

namespace todo_list_backend.Services.Project
{
    public class ProjectActivityService : IProjectActivityService
    {
        private IActivityLogService _activityLogService;
        private IServiceProvider _serviceProvider;

        public ProjectActivityService(IActivityLogService activityLogService, IServiceProvider serviceProvider)
        {
            _activityLogService = activityLogService;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ProjectActivityDto> GetProjectActivity(int projectId, int skip, int take)
        {
            var recognisedCategories = new HashSet<string>(new string[] { 
                Constants.ActivityLogCategories.TASK_ADDED,
                Constants.ActivityLogCategories.MEMBER_ADDED
            });

            IEnumerable<IComposibleProjectActivity> reports = 
                _activityLogService
                    .Select<ActivityLogItem>(item => item, item => recognisedCategories.Contains(item.Category), skip, take)
                    .Select(item => {
                        switch (item.Category)
                        {
                            case Constants.ActivityLogCategories.TASK_ADDED:
                                return new TaskAddedReport(item);
                            case Constants.ActivityLogCategories.MEMBER_ADDED:
                                return new MemberAddedReport(item);
                            default:
                                throw new Exception("Unrecognised activity category");
                        }
                    });

            return reports.Select(report => report.ToProjectActivity(_serviceProvider));
        }
    }
}
