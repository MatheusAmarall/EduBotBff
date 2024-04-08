using Microsoft.AspNetCore.Mvc;
using MediatR;
using EduBot.Application.Interactors.Bot.SendMessage;

namespace CleanArch.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase {
        private IMediator _mediator;
        public BotController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage([FromBody] SendMessageCommand request) {
            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}
