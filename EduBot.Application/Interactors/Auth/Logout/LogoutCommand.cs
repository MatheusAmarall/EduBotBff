using MediatR;

namespace EduBot.Application.Interactors.Logout
{
    public class LogoutCommand : IRequest<bool>
    {
        public string Email { get; set; } = string.Empty;
    }
}
