namespace EduBot.Api.Models {
    public class UserToken {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
