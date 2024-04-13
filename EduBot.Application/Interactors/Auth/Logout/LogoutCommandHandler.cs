using EduBot.Application.Common.Interfaces;
using MediatR;

namespace EduBot.Application.Interactors.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IAuthenticate _authentication;
        public LogoutCommandHandler(IAuthenticate authentication)
        {
            _authentication = authentication;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _authentication.Logout();
            return true;
        }
    }
}
