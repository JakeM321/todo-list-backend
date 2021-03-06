﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Project.Record
{
    public class ProjectTaskRecord
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
