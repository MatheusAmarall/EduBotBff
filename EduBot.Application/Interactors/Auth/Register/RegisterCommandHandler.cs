using EduBot.Application.Common.Interfaces;
using EduBot.Domain.Entities;
using MediatR;

namespace EduBot.Application.Interactors.Register {
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool> {
        private readonly IAuthenticate _authentication;
        public RegisterCommandHandler(IAuthenticate authentication) {
            _authentication = authentication;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken) {
            return await _authentication.RegisterUser(new User(request.isAdmin, request.Matricula, request.Email, request.Password, request.ConfirmPassword));
        }
    }
}
