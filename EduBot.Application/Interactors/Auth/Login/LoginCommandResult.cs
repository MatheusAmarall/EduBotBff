namespace EduBot.Application.Interactors.Auth.Login {
    public class LoginCommandResult {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
