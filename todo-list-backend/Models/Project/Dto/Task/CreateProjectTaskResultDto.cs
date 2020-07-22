using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Project.Dto.Task
{
    public class CreateProjectTaskResultDto
    {
        public int Id { get; set; }
        public bool ValidUser { get; set; }
    }
}
