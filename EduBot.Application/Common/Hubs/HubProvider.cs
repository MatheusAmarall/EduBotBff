using EduBot.Application.Common.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace EduBot.Application.Common.Hubs {
    public class HubProvider : Hub<IHubProvider> {
        public async Task SendMessage(MessageDto message) {
            await Clients.All.ReceivedMessage(message);
        }
    }
}
