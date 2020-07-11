using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Types
{
    public class UserToken
    {
        public int userId { get; set; }
        public long exp { get; set; }
    }
}
