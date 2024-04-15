namespace EduBot.Application.Common.DTOs {
    public class UserInfoResultDto {
        public UserInfoResultDto(string? email, string? role) 
        { 
            Email = email;
            Role = role;
        }
        public string? Email { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
    }
}
