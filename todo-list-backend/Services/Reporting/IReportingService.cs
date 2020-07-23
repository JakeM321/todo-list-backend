using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Reporting.Dto;

namespace todo_list_backend.Services.Reporting
{
    public interface IReportingService
    {
        void Report(IReportable reportable, int[] affectedUserIds);
    }
}
