using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_list_backend.Models.Auth;
using todo_list_backend.Repositories;
using todo_list_backend.Services;

namespace todo_list_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("email-login")]
        public IActionResult EmailLogin([FromBody] EmailLoginPayload payload)
        {
            var result = _authenticationService.AuthenticateEmailAndPassword(payload.email, payload.password);

            return new JsonResult(new
            {
                valid = result.Accepted,
                token = result.Token,
                email = result.User.Email,
                displayName = result.User.DisplayName
            });
        }
    }
}
