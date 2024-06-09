namespace EduBot.Application.Common.DTOs {
    public class MessageDto {
        public string NomeUsuario { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public bool ToBot { get; set; }
        public IEnumerable<ButtonDto> Buttons { get; set; } = new List<ButtonDto>();
    }
}
