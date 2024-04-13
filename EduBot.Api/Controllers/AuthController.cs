using EduBot.Application.Interactors.Login;
using EduBot.Application.Interactors.Register;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using EduBot.Application.Interactors.Logout;

namespace EduBot.Api.Controllers {
    public class AuthController : ApiController {
        public AuthController(IMediator mediator)
            : base(mediator) { }



        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginCommand userInfo) {
            ErrorOr<Unit> result = await CommandAsync(userInfo)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Created($"Usuário {userInfo.Email} logado com sucesso");
        }
        

        [HttpPost("CreatedUser")]
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

            return result.IsError ? Problem(result.Errors) : Created($"Usuário {userInfo.Email} deslogado com sucesso");
        }
    }
}
