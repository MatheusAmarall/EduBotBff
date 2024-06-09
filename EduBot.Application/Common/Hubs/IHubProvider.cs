using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Common.Hubs {
    public interface IHubProvider {
        Task ReceivedMessage(List<MessageDto> messages);
        Task MessageHistory(List<MessageHistoryDto> message);
        Task StartService(string nomeUsuario);
        Task EndService(string nomeUsuario);
    }
}
