namespace EduBot.Application.Interactors.Bot.CreateIntent {
    public class CreateIntentCommandResult {
        public string SenderId { get; set; } = string.Empty;
        public List<object> Events { get; set; } = new();
    }
}
