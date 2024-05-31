namespace EduBot.Domain.Entities
{
    public class Conversa : Entity
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public List<Message> Mensagens { get; set; } = new List<Message>();
    }

    public class Message {
        public string Sender { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
