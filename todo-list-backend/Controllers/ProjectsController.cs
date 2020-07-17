using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_list_backend.Models.Project.Dto;
using todo_list_backend.Models.User;
using todo_list_backend.Services.Project;

namespace todo_list_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private IActionResult withUser(HttpRequest request, Func<UserDto, IActionResult> callback)
        {
            var items = request.HttpContext.Items;
            if (items.ContainsKey("user"))
            {
                var user = (UserDto)items["user"];
                return callback.Invoke(user);
            } else
            {
                return ValidationProblem();
            }
        }

        [HttpPost]
        [Route("new")]
        public IActionResult New(CreateProjectDto dto)
        {
            return withUser(Request, user =>
            {
                var result = _projectService.CreateProject(user.Id, dto);
                return new JsonResult(result);
            });
        }

        [HttpGet]
        [Route("list")]
        public IActionResult List([FromQuery] int skip, [FromQuery] int take)
        {
            return withUser(Request, user =>
            {
                var result = _projectService.ListProjects(user.Id, skip, take);
                return new JsonResult(result);
            });
        }

        [HttpGet]
        [Route("tasks")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult Tasks([FromQuery] int projectId, [FromQuery] int skip, [FromQuery] int take)
        {
            var result = _projectService.ListProjectTasks(projectId, skip, take);
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("user-tasks")]
        public IActionResult UserTasks([FromQuery] int skip, [FromQuery] int take)
        {
            return withUser(Request, user =>
            {
                var result = _projectService.ListUserTasks(user.Id, skip, take);
                return new JsonResult(result);
            });
        }

        [HttpPost]
        [Route("tasks/create")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult CreateTask([FromQuery] int projectId, CreateProjectTaskDto dto)
        {
            return withUser(Request, user =>
            {
                var result = _projectService.CreateProjectTask(projectId, dto);
                return new JsonResult(result);
            });
        }
    }
}
