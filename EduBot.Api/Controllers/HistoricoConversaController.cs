using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EduBot.Application.Interactors.HistoricoConversa.GetAllMessages;

namespace EduBot.Api.Controllers {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HistoricoConversaController : ApiController {
        public HistoricoConversaController(IMediator mediator)
            : base(mediator) { }

        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages() {
            ErrorOr<IEnumerable<GetAllMessagesQueryResult>> result = await QueryAsync(new GetAllMessagesQuery())
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
