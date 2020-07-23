using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Project.Dto.Membership
{
    public class ProjectMemberDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
