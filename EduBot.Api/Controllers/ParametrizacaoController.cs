using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EduBot.Application.Interactors.Parametrizacao.CreateNewStory;
using EduBot.Application.Interactors.Parametrizacao.GetFuncionalidadesParametrizadas;

namespace EduBot.Api.Controllers {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParametrizacaoController : ApiController {
        public ParametrizacaoController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("CreateNewStory")]
        public async Task<IActionResult> CreateNewStory([FromBody] CreateNewStoryCommand request) {
            ErrorOr<Unit> result = await CommandAsync(request)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }

        [HttpGet("FuncionalidadesParametrizadas")]
        public async Task<IActionResult> GetFuncionalidadesParametrizadas() {
            ErrorOr<IEnumerable<GetFuncionalidadesParametrizadasQueryResult>> result = 
                await QueryAsync(new GetFuncionalidadesParametrizadasQuery())
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
