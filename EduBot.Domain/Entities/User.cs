namespace EduBot.Domain.Entities
{
    public class User
    {
        public User(bool isAdmin, string matricula, string email, string password, string confirmPassword)
        {

            IsAdmin = isAdmin;
            Matricula = matricula;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;

        }
        public bool IsAdmin { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
