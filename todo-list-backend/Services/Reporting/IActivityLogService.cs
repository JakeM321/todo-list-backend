using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Reporting.Dto;
using todo_list_backend.Models.Reporting.Record;

namespace todo_list_backend.Services.Reporting
{
    public interface IActivityLogService
    {
        void LogActivity(ILoggableActivity activity);
        IEnumerable<T> Select<T>(Func<ActivityLogItem, T> converter, Func<T, bool> selector, int skip, int take);
    }
}
