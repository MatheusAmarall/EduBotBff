namespace EduBot.Application.Interactors.HistoricoConversa.GetMessages {
    public class GetMessagesQueryResult {
        public string NomeUsuario { get; set; } = string.Empty;
        public List<MessageResult> Mensagens { get; set; } = new List<MessageResult>();
    }

    public class MessageResult {
        public string Sender { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
