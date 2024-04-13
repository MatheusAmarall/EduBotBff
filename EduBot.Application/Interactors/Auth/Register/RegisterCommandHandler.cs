using EduBot.Application.Common.Interfaces;
using EduBot.Domain.Entities;
using MediatR;

namespace EduBot.Application.Interactors.Register {
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<Unit>> {
        private readonly IAuthenticate _authentication;
        public RegisterCommandHandler(IAuthenticate authentication) {
            _authentication = authentication;
        }

        public async Task<ErrorOr<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken) {
            try {
                await _authentication.RegisterUser(new User(request.isAdmin, request.Matricula, request.Email, request.Password, request.ConfirmPassword));

                return Unit.Value;
            }
            catch(Exception ex) {
                return Error.Validation(description: ex.Message);
            }
            
        }
    }
}
