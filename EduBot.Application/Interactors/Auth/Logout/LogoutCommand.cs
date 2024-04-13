using MediatR;

namespace EduBot.Application.Interactors.Logout
{
    public class LogoutCommand : IRequest<ErrorOr<Unit>>
    {
        public string Email { get; set; } = string.Empty;
    }
}
