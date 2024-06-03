using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Common.Interfaces {
    public interface IMessageService {
        Task SaveMessageAsync(MessageDto message);
        Task<List<MessageHistoryDto>> MessageHistoryAsync();
    }
}
