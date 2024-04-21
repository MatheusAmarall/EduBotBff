namespace EduBot.Application.Interactors.Bot.GetMessages {
    public class GetMessagesQueryResult {
        public string SenderId { get; set; } = string.Empty;
        public List<object> Events { get; set; } = new();
    }
}
