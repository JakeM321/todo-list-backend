using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Reporting.Record
{
    public class ActivityLogItem : ActivityLogRecord
    {
        public Dictionary<string, string> Values { get; set; }
    }
}
