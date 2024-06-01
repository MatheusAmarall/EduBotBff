using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduBot.Application.Interactors.Bot.SendMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EduBot.Application.Interactors.Bot.GetMessages;
using EduBot.Application.Interactors.Bot.GetFuncionalidadesUtilizadas;

namespace EduBot.Api.Controllers {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BotController : ApiController {
        public BotController(IMediator mediator)
            : base(mediator) { }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand request) {
            ErrorOr<IEnumerable<SendMessageCommandResult>> result = await CommandAsync(request)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }

        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages(string email) {
            ErrorOr<GetMessagesQueryResult> result = await QueryAsync(new GetMessagesQuery(email))
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }

        [HttpGet("GetFuncionalidadesUtilizadas")]
        public async Task<IActionResult> GetFuncionalidadesUtilizadas(string email) {
            ErrorOr<IEnumerable<GetFuncionalidadesUtilizadasQueryResult>> result =
                await QueryAsync(new GetFuncionalidadesUtilizadasQuery(email))
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
