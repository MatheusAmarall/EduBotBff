using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Application.Common.Interfaces;
using EduBot.Application.Common.Services;
using MediatR;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ErrorOr<IEnumerable<SendMessageCommandResult>>> {
        private readonly IRasaService _rasaService;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        public SendMessageCommandHandler(IRasaService rasaService, IMapper mapper, IMessageService messageService) {
            _rasaService = rasaService;
            _mapper = mapper;
            _messageService = messageService;
        }

        public async Task<ErrorOr<IEnumerable<SendMessageCommandResult>>> Handle(SendMessageCommand request, CancellationToken cancellationToken) {
            try {
                var sendMessageRequestDto = new SendMessageRequestDto() 
                {
                    Message = request.Mensagem,
                    Sender = request.Sender,
                };

                var userMessage = new MessageDto() {
                    NomeUsuario = request.NomeUsuario,
                    Body = request.Mensagem,
                    Role = request.Role,
                    Sender = request.Sender
                };

                await _messageService.SaveMessageAsync(userMessage);

                var result = await _rasaService.SendMessageAsync(sendMessageRequestDto);

                result.ToList().ForEach(async r => {
                    var botMessage = new MessageDto() {
                        NomeUsuario = request.NomeUsuario,
                        Body = r.Text,
                        Role = "Bot",
                        Sender = "EduBot"
                    };

                    await _messageService.SaveMessageAsync(botMessage);
                });

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
