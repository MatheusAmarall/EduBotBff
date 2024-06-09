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
            bool activeBot = await _messageService.IsActiveBotAsync(message.NomeUsuario);
            if(activeBot) {
                var messagesDtos = await _messageService.SendMessageToBotAsync(message);
                await Clients.All.ReceivedMessage(messagesDtos);
                List<MessageHistoryDto> messageHistory = await _messageService.MessageHistoryAsync();
                await Clients.All.MessageHistory(messageHistory);
            }
            else {
                await _messageService.SaveMessageAsync(new List<MessageDto>() { message });
                await Clients.All.ReceivedMessage(new List<MessageDto>() { message });
                List<MessageHistoryDto> messageHistory = await _messageService.MessageHistoryAsync();
                await Clients.All.MessageHistory(messageHistory);
            }
        }

        public async Task ActivateBot(string nomeUsuario) {
            await _messageService.ActivateBotAsync(nomeUsuario);
            await Clients.All.EndService(nomeUsuario);
        }

        public async Task DisableBot(string nomeUsuario) {
            await _messageService.DisableBotAsync(nomeUsuario);
            await Clients.All.StartService(nomeUsuario);
        }
    }
}
