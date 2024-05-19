using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Common.Hubs {
    public interface IHubProvider {
        Task ReceivedMessage(MessageDto message);
    }
}
