using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Models.User;

namespace todo_list_backend.Models.Project.Dto
{
    public class ProjectTaskDto
    {
        public ProjectTaskDto() { }
        
        public ProjectTaskDto(ProjectTaskRecord record, UserRecord assignedTo, bool completed) {
            Id = record.Id;
            ProjectId = record.ProjectId;
            Label = record.Label;
            Description = record.Description;
            AssignedTo = new ProjectMemberDto
            {
                DisplayName = assignedTo.DisplayName,
                Email = assignedTo.Email
            };
            Completed = completed;
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public ProjectMemberDto AssignedTo { get; set; }
        public bool Completed { get; set; }
    }
}
