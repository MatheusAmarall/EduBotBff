using AutoMapper;
using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Interactors.HistoricoConversa.GetAllMessages {
    public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, ErrorOr<IEnumerable<GetAllMessagesQueryResult>>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IEnumerable<GetAllMessagesQueryResult>>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken) {
            try {
                var result = await _unitOfWork.Conversas.GetConversasUsuarios();

                List<GetAllMessagesQueryResult> getAllMessagesResult =
                        _mapper.Map<List<GetAllMessagesQueryResult>>(result);

                return getAllMessagesResult;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
