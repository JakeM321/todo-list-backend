using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Reporting.Dto;
using todo_list_backend.Models.Reporting.Record;
using todo_list_backend.Repositories;

namespace todo_list_backend.Services.Reporting
{
    public class ActivityLogService : IActivityLogService
    {
        private IActivityLogRepository _activityLogRepository;

        public ActivityLogService(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }
        
        public void LogActivity(ILoggableActivity activity)
        {
            var record = activity.ToRecord();
            _activityLogRepository.Save(record);
        }

        public IEnumerable<T> Select<T>(Func<ActivityLogItem, T> converter, Func<T, bool> selector, int skip, int take)
        {
            return _activityLogRepository
                .Select(item =>
                {
                    T converted = converter.Invoke(item);
                    return selector.Invoke(converted);
                }, skip, take)
                .Select(item => converter.Invoke(item));
        }
    }
}
