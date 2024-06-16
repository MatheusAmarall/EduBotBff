using AutoMapper;
using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Interactors.Agendamento.RetornaAgendamentos {
    public class RetornaAgendamentosQueryHandler : IRequestHandler<RetornaAgendamentosQuery,
        ErrorOr<IEnumerable<RetornaAgendamentosQueryResult>>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RetornaAgendamentosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IEnumerable<RetornaAgendamentosQueryResult>>> Handle(RetornaAgendamentosQuery request, CancellationToken cancellationToken) {
            try {
                var agendamentos = await _unitOfWork.Agendamentos.GetProximosAgendamentos();

                if (!agendamentos.Any()) {
                    return new List<RetornaAgendamentosQueryResult>();
                }

                List<RetornaAgendamentosQueryResult> agendamentosResult =
                        _mapper.Map<List<RetornaAgendamentosQueryResult>>(agendamentos);

                return agendamentosResult;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
