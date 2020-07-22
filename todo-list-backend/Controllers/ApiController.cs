using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;

namespace todo_list_backend.Controllers
{
    public class ApiController: ControllerBase
    {
        protected IActionResult withUser(HttpRequest request, Func<UserDto, IActionResult> callback)
        {
            var items = request.HttpContext.Items;
            if (items.ContainsKey("user"))
            {
                var user = (UserDto)items["user"];
                return callback.Invoke(user);
            }
            else
            {
                return ValidationProblem();
            }
        }
    }
}
