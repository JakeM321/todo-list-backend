using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Project.Dto
{
    public class CreateProjectTaskDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
