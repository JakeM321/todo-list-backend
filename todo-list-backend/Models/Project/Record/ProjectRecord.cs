using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Project.Record
{
    public class ProjectRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; }
    }
}
