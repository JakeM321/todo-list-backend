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
        private ILoginService _authenticationService;
        private IRegistrationService _registrationService;

        public AuthController(ILoginService authenticationService, IRegistrationService registrationService)
        {
            _authenticationService = authenticationService;
            _registrationService = registrationService;
        }

        [HttpPost]
        [Route("email-login")]
        public IActionResult EmailLogin([FromBody] EmailLoginDto payload)
        {
            var result = _authenticationService.AuthenticateEmailAndPassword(payload);

            return new JsonResult(new
            {
                valid = result.Accepted,
                token = result.Token,
                email = result.User.Get(user => user.Email, () => ""),
                displayName = result.User.Get(user => user.DisplayName, () => "")
            });
        }

        [HttpPost]
        [Route("email-register")]
        public IActionResult EmailRegister([FromBody] EmailRegisterDto payload)
        {
            var result = _registrationService.RegisterWithEmailAndPassword(payload);

            return new JsonResult(new
            {
                valid = result.UserAvailable,
                accountAlreadyInUse = !result.UserAvailable,
                token = result.Token,
                email = result.User.Get(user => user.Email, () => ""),
                displayName = result.User.Get(user => user.DisplayName, () => "")
            }) ;
        }
    }
}
