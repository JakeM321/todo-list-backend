using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Types
{
    public class JsonOption<T>
    {
        public bool some { get; set; }
        public T item { get; set; }
    }
}
