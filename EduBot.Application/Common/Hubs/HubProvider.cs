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
            if(message.ToBot) {
                List<MessageDto> messageDtos = await _messageService.SendMessageToBotAsync(message);
                await Clients.All.ReceivedMessage(messageDtos);
                List<MessageHistoryDto> messageHistory = await _messageService.MessageHistoryAsync();
                await Clients.All.MessageHistory(messageHistory);
            }
            else {
                await _messageService.SaveMessageAsync(message);
                await Clients.All.ReceivedMessage(new List<MessageDto>() { message });
                List<MessageHistoryDto> messageHistory = await _messageService.MessageHistoryAsync();
                await Clients.All.MessageHistory(messageHistory);
            }
        }
    }
}
