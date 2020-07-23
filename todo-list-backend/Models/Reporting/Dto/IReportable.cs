using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Reporting.Dto
{
    public interface IReportable : ILoggableActivity, IComposibleNotification
    {
    }
}
