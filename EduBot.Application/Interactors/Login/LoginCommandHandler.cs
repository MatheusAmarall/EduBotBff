using EduBot.Application.Common.Interfaces;
using MediatR;

namespace EduBot.Application.Interactors.Login {
    public class LoginCommandHandler : IRequestHandler<LoginCommand, bool> {
        private readonly IAuthenticate _authentication;
        public LoginCommandHandler(IAuthenticate authentication) {
            _authentication = authentication;
        }

        public async Task<bool> Handle(LoginCommand request, CancellationToken cancellationToken) {
            return await _authentication.Authenticate(request.Email, request.Password);
        }
    }
}
