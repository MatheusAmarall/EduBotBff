using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Common.Interfaces {
    public interface IMessageService {
        Task SaveMessageAsync(List<MessageDto> messages);
        Task<List<MessageHistoryDto>> MessageHistoryAsync();
        Task<List<MessageDto>> SendMessageToBotAsync(MessageDto request);
        Task DisableBotAsync(string nomeUsuario);
        Task ActivateBotAsync(string nomeUsuario);
        Task<bool> IsActiveBotAsync(string nomeUsuario);
    }
}
