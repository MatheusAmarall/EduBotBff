using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Application.Common.Interfaces;
using EduBot.Domain.Entities;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Common.Services {
    public class MessageService : IMessageService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessageService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task SaveMessageAsync(MessageDto message) {
            try {
                var result = await _unitOfWork.Conversas.GetConversaByNome(message.NomeUsuario);

                var mensagem = new Message() {
                    Sender = message.Sender,
                    Body = message.Mensagem
                };

                if (result is null) {
                    var conversa = new Conversa() {
                        NomeUsuario = message.NomeUsuario,
                        Role = message.Role
                    };
                    
                    conversa.Mensagens.Add(mensagem);

                    _unitOfWork.Conversas.Add(conversa, CancellationToken.None);
                }
                else {
                    result.Mensagens.Add(mensagem);

                    _unitOfWork.Conversas.Update(result, CancellationToken.None);
                }

                await _unitOfWork.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MessageHistoryDto>> MessageHistoryAsync() {
            try {
                var result = await _unitOfWork.Conversas.GetConversasUsuarios();

                List<MessageHistoryDto> messageHistoryResult =
                        _mapper.Map<List<MessageHistoryDto>>(result);

                return messageHistoryResult;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
