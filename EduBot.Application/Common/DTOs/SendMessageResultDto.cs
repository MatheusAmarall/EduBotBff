using System.Text.Json.Serialization;

namespace EduBot.Application.Common.DTOs {
    public class SendMessageResultDto {
        [JsonPropertyName("recipient_id")]
        public string RecipientId { get; set; } = string.Empty;
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
        [JsonPropertyName("buttons")]
        public IEnumerable<ButtonDto> Buttons { get; set; } = new List<ButtonDto>();
    }
    public class ButtonDto {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("payload")]
        public string Payload { get; set; } = string.Empty;
    }
}
