using MediatR;

namespace EduBot.Application.Interactors.Parametrizacao.GetFuncionalidadesParametrizadas {
    public record GetFuncionalidadesParametrizadasQuery() : IRequest<ErrorOr<IEnumerable<GetFuncionalidadesParametrizadasQueryResult>>>;
}
