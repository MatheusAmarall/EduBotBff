using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Application.Common.Interfaces;
using EduBot.Domain.Entities;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Common.Services {
    public class MessageService : IMessageService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRasaService _rasaService;
        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, IRasaService rasaService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _rasaService = rasaService;
        }
        public async Task SaveMessageAsync(List<MessageDto> messages) {
            try {
                var result = await _unitOfWork.Conversas.GetConversaByNome(messages.First().NomeUsuario);

                var mensagens = new List<Message>();
                messages.ForEach(m => {
                    mensagens.Add(
                        new Message() {
                            Sender = m.Sender,
                            Body = m.Body
                        }
                    );
                });

                if (result is null) {
                    var conversa = new Conversa() {
                        NomeUsuario = messages.First().NomeUsuario,
                        Role = messages.First().Role
                    };
                    
                    conversa.Mensagens.AddRange(mensagens);

                    _unitOfWork.Conversas.Add(conversa, CancellationToken.None);
                }
                else {
                    result.Mensagens.AddRange(mensagens);

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

        public async Task<List<MessageDto>> SendMessageToBotAsync(MessageDto request) {
            try {
                var sendMessageRequestDto = new SendMessageRequestDto() {
                    Message = request.Body,
                    Sender = request.Sender,
                };

                var userMessage = new MessageDto() {
                    NomeUsuario = request.NomeUsuario,
                    Body = request.Body,
                    Role = request.Role,
                    Sender = request.Sender
                };

                var result = await _rasaService.SendMessageAsync(sendMessageRequestDto);
                var resultDto = new List<MessageDto> {
                    userMessage
                };

                result.ToList().ForEach(r => {
                    var botMessage = new MessageDto() {
                        NomeUsuario = request.NomeUsuario,
                        Body = r.Text,
                        Buttons = r.Buttons,
                        Role = "Bot",
                        Sender = "EduBot"
                    };

                    resultDto.Add(botMessage);
                });

                await SaveMessageAsync(resultDto);

                return resultDto;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task DisableBotAsync(string nomeUsuario) {
            try {
                var result = await _unitOfWork.Conversas.GetConversaByNome(nomeUsuario);

                if(result != null) {
                    result.ActiveBot = false;

                    _unitOfWork.Conversas.Update(result, CancellationToken.None);

                    await _unitOfWork.SaveChangesAsync(CancellationToken.None);
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task ActivateBotAsync(string nomeUsuario) {
            try {
                var result = await _unitOfWork.Conversas.GetConversaByNome(nomeUsuario);

                if (result != null) {
                    result.ActiveBot = true;

                    _unitOfWork.Conversas.Update(result, CancellationToken.None);

                    await _unitOfWork.SaveChangesAsync(CancellationToken.None);
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsActiveBotAsync(string nomeUsuario) {
            try {
                var result = await _unitOfWork.Conversas.GetConversaByNome(nomeUsuario);

                if (result != null) {
                    return result.ActiveBot;
                }
                else {
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
