using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;

namespace todo_list_backend.Models.Project.Dto
{
    public class ProjectDto: ProjectRecord
    {
        public ProjectDto() { }

        public ProjectDto(ProjectRecord record)
        {
            Id = record.Id;
            UserId = record.UserId;
            Title = record.Title;
            Colour = record.Colour;
        }
    }
}
