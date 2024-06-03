namespace EduBot.Application.Common.DTOs {
    public class MessageHistoryDto {
        public string NomeUsuario { get; set; } = string.Empty;
        public List<MessageResultHistoryDto> Mensagens { get; set; } = new List<MessageResultHistoryDto>();
    }

    public class MessageResultHistoryDto {
        public string Sender { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
