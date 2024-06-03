using AutoMapper;
using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Interactors.HistoricoConversa.GetMessages {
    public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, ErrorOr<GetMessagesQueryResult>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<GetMessagesQueryResult>> Handle(GetMessagesQuery request, CancellationToken cancellationToken) {
            try {
                var result = await _unitOfWork.Conversas.GetConversaByNome(request.Email);

                if(result is null) {
                    return new ErrorOr<GetMessagesQueryResult>();
                }

                GetMessagesQueryResult getMessagesResult =
                        _mapper.Map<GetMessagesQueryResult>(result);

                return getMessagesResult;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
