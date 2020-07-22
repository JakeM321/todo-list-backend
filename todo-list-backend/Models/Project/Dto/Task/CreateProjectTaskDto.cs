using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Membership;

namespace todo_list_backend.Models.Project.Dto.Task
{
    public class CreateProjectTaskDto
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public ProjectMemberDto AssignedTo { get; set; }
    }
}
