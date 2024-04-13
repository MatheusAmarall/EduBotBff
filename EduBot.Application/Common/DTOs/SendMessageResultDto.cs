using System.Text.Json.Serialization;

namespace EduBot.Application.Common.DTOs {
    public class SendMessageResultDto {
        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; } = string.Empty;
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
