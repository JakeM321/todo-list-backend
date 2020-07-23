using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Reporting.Record;

namespace todo_list_backend.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private AppDbContext _db;
        private ILogger _logger;

        public ActivityLogRepository(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger("Activity Log Respository");
        }

        public void Save(ActivityLogItem item)
        {
            var record = _db.ActivityLog.Add(item);
            _db.SaveChanges();

            var id = record.Entity.Id;

            item.Values.ToList().ForEach(pair => _db.ActivityLogValues.Add(new ActivityLogValue
            {
                ActivityLogId = id,
                Key = pair.Key,
                Value = pair.Value
            }));

            _db.SaveChanges();
        }

        public IEnumerable<ActivityLogItem> Select(Func<ActivityLogItem, bool> predicate, int skip, int take)
        {
            var query = (
                from logItem in _db.ActivityLog
                join valuePair in _db.ActivityLogValues
                on logItem.Id equals valuePair.ActivityLogId
                select new { logItem, valuePair }
            ).ToList();

            var grouped = query
                .GroupBy(i => i.logItem).ToList();

            var result = grouped
                .Select(i => new ActivityLogItem
                {
                    Id = i.Key.Id,
                    Category = i.Key.Category,
                    ProjectId = i.Key.ProjectId,
                    UserId = i.Key.UserId,
                    Values = i.ToDictionary(grouping => grouping.valuePair.Key, grouping => grouping.valuePair.Value)
                })
                .Where(predicate)
                .Skip(skip)
                .Take(take)
                .ToArray();

            return result;
        }
    }
}
