using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Reporting.Record;

namespace todo_list_backend.Repositories
{
    public interface IActivityLogRepository
    {
        void Save(ActivityLogItem record);
        IEnumerable<ActivityLogItem> Select(Func<ActivityLogItem, bool> predicate, int skip, int take);
    }
}
