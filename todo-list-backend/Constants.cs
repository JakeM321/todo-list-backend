using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend
{
    public static class Constants
    {
        public const int TOKEN_REFRESH_WINDOW_MINUTES = 30;
        public const int TOKEN_LIFESPAN_MINUTES = 120;

        public static class ActivityLogCategories
        {
            public const string TASK_ADDED = "TASK_ADDED";
        }
    }
}
