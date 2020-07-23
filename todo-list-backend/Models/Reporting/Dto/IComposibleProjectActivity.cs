using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Activity;

namespace todo_list_backend.Models.Reporting.Dto
{
    public interface IComposibleProjectActivity
    {
        ProjectActivityDto ToProjectActivity(IServiceProvider services);
    }
}
