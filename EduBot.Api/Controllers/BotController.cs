using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EduBot.Application.Interactors.Bot.GetFuncionalidadesUtilizadas;

namespace EduBot.Api.Controllers {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BotController : ApiController {
        public BotController(IMediator mediator)
            : base(mediator) { }

        [HttpGet("GetFuncionalidadesUtilizadas")]
        public async Task<IActionResult> GetFuncionalidadesUtilizadas(string email) {
            ErrorOr<IEnumerable<GetFuncionalidadesUtilizadasQueryResult>> result =
                await QueryAsync(new GetFuncionalidadesUtilizadasQuery(email))
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
