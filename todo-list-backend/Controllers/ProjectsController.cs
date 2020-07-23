using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_list_backend.Models.Project.Dto.Membership;
using todo_list_backend.Models.Project.Dto.Project;
using todo_list_backend.Models.Project.Dto.Task;
using todo_list_backend.Models.Reporting.Dto;
using todo_list_backend.Models.User;
using todo_list_backend.Services;
using todo_list_backend.Services.Project;
using todo_list_backend.Services.Reporting;
using todo_list_backend.Utils;

namespace todo_list_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ApiController
    {
        private IProjectService _projectService;
        private IProjectTaskService _projectTaskService;
        private IProjectMembershipService _projectMembershipService;
        private IUserService _userService;
        private IReportingService _reportingService;
        private IProjectActivityService _projectActivityService;

        public ProjectsController(
            IProjectService projectService,
            IProjectTaskService projectTaskService,
            IProjectMembershipService projectMembershipService,
            IUserService userService,
            IReportingService reportingService,
            IProjectActivityService projectActivityService)
        {
            _projectService = projectService;
            _projectTaskService = projectTaskService;
            _projectMembershipService = projectMembershipService;
            _userService = userService;
            _reportingService = reportingService;
            _projectActivityService = projectActivityService;
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
        [Route("info")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult ProjectInfo([FromQuery] int projectId)
        {
            return withUser(Request, user =>
            {
                var result = _projectService.GetInfo(user.Id, projectId);
                return new JsonResult(result.ToJson());
            });
        }

        [HttpGet]
        [Route("tasks")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult Tasks([FromQuery] int projectId, [FromQuery] int skip, [FromQuery] int take)
        {
            var result = _projectTaskService.ListProjectTasks(projectId, skip, take);
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("user-tasks")]
        public IActionResult UserTasks([FromQuery] int skip, [FromQuery] int take)
        {
            return withUser(Request, user =>
            {
                var result = _projectTaskService.ListUserTasks(user.Id, skip, take);
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
                var result = _projectTaskService.CreateProjectTask(projectId, dto);

                if (result.ValidUser)
                {
                    var recipients = _projectMembershipService.ListMembers(projectId).Select(i => i.UserId).Where(id => id != user.Id).ToArray();

                    _reportingService.Report(new TaskAddedReport
                    {
                        AddedByUserId = user.Id,
                        ProjectId = projectId,
                        ProjectTaskId = result.Id
                    }, recipients);
                }

                return result.ValidUser
                    ? new JsonResult(new { result.Id })
                    : ValidationProblem("Cannot assign a task on this project to a non-member");
            });
        }

        [HttpGet]
        [Route("members")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult ListMembers([FromQuery] int projectId)
        {
            var result = _projectMembershipService.ListMembers(projectId);
            return new JsonResult(result);
        }

        [HttpPatch]
        [Route("set-favourite")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult SetFavourite([FromQuery] int projectId, [FromBody] ToggleFavouriteDto dto)
        {
            return withUser(Request, user =>
            {
                _projectService.SetFavourite(user.Id, projectId, dto.Favourite);
                return Ok();
            });
        }

        [HttpPatch]
        [Route("tasks/set-completion")]
        [Authorize(Policy = "HasProjectMembership")]
        [Authorize(Policy = "TaskBelongsToProject")]
        public IActionResult SetCompletion([FromQuery] int projectTaskId, [FromBody] ToggleCompletionDto dto)
        {
            return withUser(Request, user =>
            {
                _projectTaskService.SetCompletion(user.Id, projectTaskId, dto.Completed);
                return Ok();
            });
        }

        [HttpPost]
        [Route("add-member")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult AddMemberToProject([FromQuery] int projectId, [FromBody] AddProjectMemberDto dto)
        {
            return _userService.FindByEmail(dto.Email).Get(user =>
            {
                _projectMembershipService.AddMember(user.Id, projectId);
                return Ok();
            }, () =>
            {
                //TODO: Implement invitation emails
                return Ok();
            });
        }

        [HttpGet]
        [Route("activity")]
        [Authorize(Policy = "HasProjectMembership")]
        public IActionResult GetActivity([FromQuery] int projectId, [FromQuery] int skip, [FromQuery] int take)
        {
            var result = _projectActivityService.GetProjectActivity(projectId, skip, take);
            return new JsonResult(result);
        }
    }
}
