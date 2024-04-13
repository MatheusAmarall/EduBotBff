using AutoMapper;
using EduBot.Application.Common.Services;
using MediatR;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ErrorOr<IEnumerable<SendMessageCommandResult>>> {
        private readonly IRasaService _rasaService;
        private readonly IMapper _mapper;
        public SendMessageCommandHandler(IRasaService rasaService, IMapper mapper) {
            _rasaService = rasaService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IEnumerable<SendMessageCommandResult>>> Handle(SendMessageCommand request, CancellationToken cancellationToken) {
            try {
                var result = await _rasaService.SendMessageAsync(request);

                IEnumerable<SendMessageCommandResult> sendMessageResult =
                        _mapper.Map<IEnumerable<SendMessageCommandResult>>(result);

                return sendMessageResult.ToList();
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
