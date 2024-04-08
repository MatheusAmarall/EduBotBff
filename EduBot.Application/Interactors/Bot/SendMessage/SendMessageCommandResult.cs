using System.Text.Json.Serialization;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public class SendMessageCommandResult {
        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; } = string.Empty;
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
