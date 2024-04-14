using EduBot.Application.Interactors.Register;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduBot.Application.Interactors.Logout;
using EduBot.Application.Interactors.Auth.Login;
using EduBot.Application.Interactors.Auth.VisitorUser;

namespace EduBot.Api.Controllers {
    public class AuthController : ApiController {
        public AuthController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginCommand userInfo) {
            ErrorOr<LoginCommandResult> result = await CommandAsync(userInfo)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterCommand userInfo) {
            ErrorOr<Unit> result = await CommandAsync(userInfo)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Created($"Usuário {userInfo.Email} criado com sucesso");
        }

        [HttpPost("LogoutUser")]
        public async Task<IActionResult> LogoutUser([FromBody] LogoutCommand userInfo)
        {
            ErrorOr<Unit> result = await CommandAsync(userInfo)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok($"Usuário {userInfo.Email} deslogado com sucesso");
        }

        [HttpPost("VisitorUser")]
        public async Task<IActionResult> VisitorUser() {
            ErrorOr<VisitorUserCommandResult> result = await CommandAsync(new VisitorUserCommand())
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
