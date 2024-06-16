using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EduBot.Api.Controllers;
using EduBot.Application.Interactors.Agendamento.RealizaAgendamento;
using EduBot.Application.Interactors.Agendamento.RetornaAgendamentos;

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

        [HttpGet("RetornaAgendamentos")]
        public async Task<IActionResult> RetornaAgendamentos() {
            ErrorOr<IEnumerable<RetornaAgendamentosQueryResult>> result =
                await QueryAsync(new RetornaAgendamentosQuery())
                .ConfigureAwait(false);

            return result.IsError ? Problem(result.Errors) : Ok(result.Value);
        }
    }
}
