using MediatR;

namespace EduBot.Application.Interactors.Login {
    public class LoginCommand : IRequest<ErrorOr<Unit>> {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
