using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EduBot.Api.Controllers;
using EduBot.Application.Interactors.Agendamento.RealizaAgendamento;

namespace EduAgendamento.Api.Controllers {
    public class AgendamentoController : ApiController {
        public AgendamentoController(IMediator mediator)
            : base(mediator) { }

        [HttpPost("RealizaAgendamento")]
        public async Task<IActionResult> RealizaAgendamento(string email) {
            ErrorOr<RealizaAgendamentoCommandResult> result =
                await CommandAsync(new RealizaAgendamentoCommand(email))
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
