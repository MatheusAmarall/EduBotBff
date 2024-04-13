using EduBot.Application.Common.Interfaces;
using MediatR;

namespace EduBot.Application.Interactors.Login {
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<Unit>> {
        private readonly IAuthenticate _authentication;
        public LoginCommandHandler(IAuthenticate authentication) {
            _authentication = authentication;
        }

        public async Task<ErrorOr<Unit>> Handle(LoginCommand request, CancellationToken cancellationToken) {
            try {
                bool result = await _authentication.Authenticate(request.Email, request.Password);
                if(!result) {
                    return Error.Validation(description: "Falha ao logar, verifique os seus dados");
                }

                return Unit.Value;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
