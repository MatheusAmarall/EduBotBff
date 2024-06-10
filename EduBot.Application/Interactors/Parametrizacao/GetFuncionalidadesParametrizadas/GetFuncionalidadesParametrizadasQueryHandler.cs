using AutoMapper;
using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Interactors.Parametrizacao.GetFuncionalidadesParametrizadas {
    public class GetFuncionalidadesParametrizadasQueryHandler : IRequestHandler<GetFuncionalidadesParametrizadasQuery, ErrorOr<IEnumerable<GetFuncionalidadesParametrizadasQueryResult>>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetFuncionalidadesParametrizadasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IEnumerable<GetFuncionalidadesParametrizadasQueryResult>>> Handle(
            GetFuncionalidadesParametrizadasQuery request, 
            CancellationToken cancellationToken) {
            try {
                var result = await _unitOfWork.Funcionalidades.GetAllAsync(cancellationToken);

                List<GetFuncionalidadesParametrizadasQueryResult> getFuncionalidadesParametrizadasResult =
                        _mapper.Map<List<GetFuncionalidadesParametrizadasQueryResult>>(result);

                return getFuncionalidadesParametrizadasResult;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
