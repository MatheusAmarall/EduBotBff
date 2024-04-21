using MediatR;

namespace EduBot.Application.Interactors.Register {
    public class RegisterCommand : IRequest<ErrorOr<Unit>> {

        public bool IsAdmin { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
