using EduBot.Application.Common.Services;
using MediatR;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, IEnumerable<SendMessageCommandResult>> {
        private readonly IRasaService _rasaService;
        public SendMessageCommandHandler(IRasaService rasaService) {
            _rasaService = rasaService;
        }

        public async Task<IEnumerable<SendMessageCommandResult>> Handle(SendMessageCommand request, CancellationToken cancellationToken) {
            var result = await _rasaService.SendMessageAsync(request);

            return result;
        }
    }
}
