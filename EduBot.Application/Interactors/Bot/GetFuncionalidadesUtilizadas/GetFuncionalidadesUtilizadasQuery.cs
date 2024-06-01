using MediatR;

namespace EduBot.Application.Interactors.Bot.GetFuncionalidadesUtilizadas {
    public record GetFuncionalidadesUtilizadasQuery(string Email) : IRequest<ErrorOr<IEnumerable<GetFuncionalidadesUtilizadasQueryResult>>>;
}
