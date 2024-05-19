namespace EduBot.Application.Interactors.HistoricoConversa.GetAllMessages {
    public class GetAllMessagesQueryResult {
        public string NomeUsuario { get; set; } = string.Empty;
        public List<MessageResult> Mensagens { get; set; } = new List<MessageResult>();
    }

    public class MessageResult {
        public string Sender { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
