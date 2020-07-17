using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Models.User;

namespace todo_list_backend.Repositories
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserRecord> Users { get; set; }
        public DbSet<UserNotificationRecord> Notifications { get; set; }
        public DbSet<ProjectRecord> Projects { get; set; }
        public DbSet<ProjectMembershipRecord> ProjectMemberships { get; set; }
        public DbSet<ProjectTaskRecord> ProjectTasks { get; set; }
    }
}
