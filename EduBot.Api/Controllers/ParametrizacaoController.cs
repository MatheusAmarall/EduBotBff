using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduBot.Application.Interactors.Bot.CreateIntent;

namespace EduBot.Api.Controllers {
    public class ParametrizacaoController : ApiController {
        public ParametrizacaoController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("CreateIntent")]
        public async Task<IActionResult> CreateIntent([FromBody] CreateIntentCommand request) {
            ErrorOr<CreateIntentCommandResult> result = await CommandAsync(request)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
