using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;

namespace todo_list_backend.Models.Project.Dto
{
    public class ProjectTaskDto: ProjectTaskRecord
    {
        public ProjectTaskDto() { }
        
        public ProjectTaskDto(ProjectTaskRecord record) {
            Id = record.Id;
            UserId = record.UserId;
            ProjectId = record.ProjectId;
            Name = record.Name;
            Description = record.Description;
        }
    }
}
