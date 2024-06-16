using MediatR;

namespace EduBot.Application.Interactors.Agendamento.RetornaAgendamentos {
    public record RetornaAgendamentosQuery() : IRequest<ErrorOr<IEnumerable<RetornaAgendamentosQueryResult>>>;
}
