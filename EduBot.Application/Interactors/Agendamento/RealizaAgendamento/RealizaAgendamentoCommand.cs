using MediatR;

namespace EduBot.Application.Interactors.Agendamento.RealizaAgendamento {
    public record RealizaAgendamentoCommand(string Email) : IRequest<ErrorOr<RealizaAgendamentoCommandResult>>;
}
