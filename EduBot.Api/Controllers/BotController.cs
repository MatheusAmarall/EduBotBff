using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduBot.Application.Interactors.Bot.SendMessage;

namespace EduBot.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ApiController {
        public BotController(IMediator mediator)
            : base(mediator) { }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand request) {
            ErrorOr<IEnumerable<SendMessageCommandResult>> result = await CommandAsync(request)
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
