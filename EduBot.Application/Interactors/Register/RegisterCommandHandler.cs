using EduBot.Application.Common.Interfaces;
using MediatR;

namespace EduBot.Application.Interactors.Register {
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool> {
        private readonly IAuthenticate _authentication;
        public RegisterCommandHandler(IAuthenticate authentication) {
            _authentication = authentication;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken) {
            return await _authentication.RegisterUser(request.Email, request.Password);
        }
    }
}
