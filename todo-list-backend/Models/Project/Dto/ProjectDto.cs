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

        public ProjectDto(ProjectRecord record, bool belongsToUser, bool isFavourite)
        {
            Id = record.Id;
            UserId = record.UserId;
            Title = record.Title;
            Colour = record.Colour;
            BelongsToUser = belongsToUser;
            IsFavourite = isFavourite;
        }

        public bool BelongsToUser { get; set; }
        public bool IsFavourite { get; set; }
    }
}
