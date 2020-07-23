using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Reporting.Record
{
    public class ActivityLogValue
    {
        public int Id { get; set; }
        public int ActivityLogId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
