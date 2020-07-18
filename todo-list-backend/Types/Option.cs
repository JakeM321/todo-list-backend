using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Types
{
    public class Option<T>
    {
        private T _item { get; set; }
        private bool _some { get; set; }

        public Option(T item)
        {
            _item = item;
            _some = true;
        }

        public Option()
        {
            _some = false;
        }

        public Option(JsonOption<T> json)
        {
            _item = json.item;
            _some = json.some;
        }

        public TReturn Get<TReturn>(Func<T, TReturn> some, Func<TReturn> none)
        {
            return _some ? some.Invoke(_item) : none.Invoke();
        }

        public void Use(Action<T> some)
        {
            Use(some, () => { });
        }

        public void Use(Action<T> some, Action none)
        {
            if (_some)
            {
                some.Invoke(_item);
            }
            else
            {
                none.Invoke();
            }
        }

        public JsonOption<T> ToJson()
        {
            return new JsonOption<T>
            {
                some = _some,
                item = _item
            };
        }

        public bool Some
        {
            get { return _some; }
        }
    }
}
