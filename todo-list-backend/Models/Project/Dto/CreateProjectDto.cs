using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Project.Dto
{
    public class CreateProjectDto
    {
        public string Title { get; set; }
        public string Colour { get; set; }
    }
}
