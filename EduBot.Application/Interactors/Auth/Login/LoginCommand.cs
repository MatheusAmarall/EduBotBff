using MediatR;

namespace EduBot.Application.Interactors.Auth.Login {
    public class LoginCommand : IRequest<ErrorOr<LoginCommandResult>> {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
