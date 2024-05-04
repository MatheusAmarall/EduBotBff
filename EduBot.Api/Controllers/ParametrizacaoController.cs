using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduBot.Application.Interactors.Bot.CreateNewStory;

namespace EduBot.Api.Controllers {
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
    }
}
