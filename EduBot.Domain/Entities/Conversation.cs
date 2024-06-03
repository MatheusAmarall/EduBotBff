using System.Text.Json.Serialization;

namespace EduBot.Domain.Entities
{
    public class Conversation : Entity
    {
        public string sender_id { get; set; } = string.Empty;
        public List<object> events { get; set; } = new();
    }

    public class ConversationSimplify {
        public string SenderId { get; set; } = string.Empty;
        public List<EventObject> Events { get; set; } = new();
    }

    public class EventObject {
        [JsonPropertyName("event")]
        public string? Event { get; set; } = string.Empty;

        [JsonPropertyName("metadata")]
        public MetadataObject? Metadata { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; } = string.Empty;
    }

    public class MetadataObject {
        [JsonPropertyName("utter_action")]
        public string? UtterAction { get; set; } = string.Empty;

        [JsonPropertyName("template")]
        public string? Template { get; set; } = string.Empty;

        [JsonPropertyName("model_id")]
        public string? ModelId { get; set; } = string.Empty;

        [JsonPropertyName("assistant_id")]
        public string? AssistantId { get; set; } = string.Empty;
    }
}
