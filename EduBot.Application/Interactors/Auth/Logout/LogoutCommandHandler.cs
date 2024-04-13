using EduBot.Application.Common.Interfaces;
using MediatR;

namespace EduBot.Application.Interactors.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<Unit>>
    {
        private readonly IAuthenticate _authentication;
        public LogoutCommandHandler(IAuthenticate authentication)
        {
            _authentication = authentication;
        }

        public async Task<ErrorOr<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try {
                await _authentication.Logout();
                return Unit.Value;
            }
            catch(Exception ex) {
                return Error.Validation(description: ex.Message);
            }
            
        }
    }
}
