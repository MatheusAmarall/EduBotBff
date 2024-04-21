namespace EduBot.Domain.Entities
{
    public class Conversation : Entity
    {
        public string sender_id { get; set; } = string.Empty;
        public List<object> events { get; set; } = new();
    }

    public class ConversationSimplify {
        public string SenderId { get; set; } = string.Empty;
        public List<object> Events { get; set; } = new();
    }
}
