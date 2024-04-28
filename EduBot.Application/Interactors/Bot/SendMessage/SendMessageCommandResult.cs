using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public class SendMessageCommandResult {
        public string RecipientId { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public IEnumerable<ButtonDto> Buttons { get; set; } = new List<ButtonDto>();
    }
}
