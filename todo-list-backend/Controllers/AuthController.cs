using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using todo_list_backend.Models.Auth;
using todo_list_backend.Models.Notification;
using todo_list_backend.Repositories;
using todo_list_backend.Services;

namespace todo_list_backend.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ApiController
    {
        private ILoginService _authenticationService;
        private IRegistrationService _registrationService;
        private IUserService _userService;
        private IConfiguration _configuration;
        private IOAuthHandlerService _oauthHandlerService;
        private IOAuthProviderService _oauthProviderService;
        private INotificationSenderService _notificationSenderService;

        public AuthController(
            ILoginService authenticationService,
            IRegistrationService registrationService,
            IUserService userService,
            IConfiguration configuration,
            IOAuthHandlerService oAuthHandlerService,
            IOAuthProviderService oAuthProviderService,
            INotificationSenderService notificationSenderService
        ) {
            _authenticationService = authenticationService;
            _registrationService = registrationService;
            _userService = userService;
            _configuration = configuration;
            _oauthHandlerService = oAuthHandlerService;
            _oauthProviderService = oAuthProviderService;
            _notificationSenderService = notificationSenderService;
        }

        private void SendWelcomeMessage(int userId)
        {
            _notificationSenderService.SendNotificationAsync(new CreateUserNotificationDto
            {
                UserId = userId,
                Subject = "Welcome",
                Message = "Welcome to the TODO list app",
                isLink = false,
                Link = ""
            }, 5000);
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

            if (result.UserAvailable)
            {
                result.User.Use(user => SendWelcomeMessage(user.Id));
            }

            return new JsonResult(new
            {
                valid = result.UserAvailable,
                token = result.Token,
                email = result.User.Get(user => user.Email, () => ""),
                displayName = result.User.Get(user => user.DisplayName, () => "")
            }) ;
        }

        [HttpGet]
        [Route("email-availability/{email}")]
        public bool EmailAvailability(string email)
        {
            return _userService.FindByEmail(email).Get(some => false, () => true);
        }

        [HttpGet]
        [Route("oauth-redirect")]
        public string OAuthRedirect()
        {
            return Utils.Authentication.Google.RedirectUrl(_configuration);
        }

        [HttpPost]
        [Route("assert")]
        public async Task<IActionResult> Assert([FromQuery] string code)
        {
            return (await _oauthProviderService.ProcessGoogleCallback(code)).Get<IActionResult>(
                user => {
                    var authentication = _oauthHandlerService.LoginOrRegister(user);

                    if (authentication.UserNewlyCreated)
                    {
                        authentication.User.Use(user => SendWelcomeMessage(user.Id));
                    }

                    return new JsonResult(new
                    {
                        valid = authentication.Accepted,
                        token = authentication.Token,
                        email = authentication.User.Get(user => user.Email, () => ""),
                        displayName = authentication.User.Get(user => user.DisplayName, () => "")
                    });
                },
                () => new JsonResult(new
                {
                    valid = false,
                    token = "",
                    email = "",
                    displayName = ""
                })
            );
        }
    }
}
