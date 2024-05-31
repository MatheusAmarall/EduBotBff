using EduBot.Application.Common.DTOs;
using EduBot.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace EduBot.Application.Common.Hubs {
    public class HubProvider : Hub<IHubProvider> {
        private readonly IMessageService _messageService;
        public HubProvider(IMessageService messageService) {
            _messageService = messageService;
        }
        public async Task SendMessage(MessageDto message) {
            await _messageService.SaveMessageAsync(message);
            await Clients.All.ReceivedMessage(message);
        }
    }
}
