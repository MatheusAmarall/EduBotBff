using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Interactors.Bot.GetFuncionalidadesUtilizadas {
    public class GetFuncionalidadesUtilizadasQueryHandler : IRequestHandler<GetFuncionalidadesUtilizadasQuery, 
        ErrorOr<IEnumerable<GetFuncionalidadesUtilizadasQueryResult>>> {
        private readonly IUnitOfWork _unitOfWork;
        public GetFuncionalidadesUtilizadasQueryHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<IEnumerable<GetFuncionalidadesUtilizadasQueryResult>>> Handle(GetFuncionalidadesUtilizadasQuery request, CancellationToken cancellationToken) {
            try {
                var results = await _unitOfWork.Conversations.GetAllEventsAsync();

                if (!results.Any()) {
                    return Error.Validation(description: "Sem registros");
                }

                var utterActions = results.SelectMany(result => result!.Events)
                    .Where(e => e.Event == "bot" && e.Metadata != null && e.Metadata.UtterAction != null)
                    .Select(e => e.Metadata!.UtterAction!.Split('_').Last())
                    .ToList();

                if (utterActions.Count == 0) {
                    return Error.Validation(description: "Sem registros");
                }

                var utterActionCounts = utterActions
                    .GroupBy(action => action)
                    .Select(group => new GetFuncionalidadesUtilizadasQueryResult {
                        Nome = group.Key,
                        Total = group.Count()
                    })
                    .ToList();

                return utterActionCounts;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
