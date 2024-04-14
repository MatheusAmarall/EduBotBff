using AutoMapper;
using EduBot.Application.Common.Interfaces;
using EduBot.Application.Common.Util;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EduBot.Application.Interactors.Auth.Login {
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginCommandResult>> {
        private readonly IAuthenticate _authentication;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public LoginCommandHandler(IAuthenticate authentication, IMapper mapper, IConfiguration configuration) {
            _authentication = authentication;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ErrorOr<LoginCommandResult>> Handle(LoginCommand request, CancellationToken cancellationToken) {
            try {
                bool result = await _authentication.Authenticate(request.Email, request.Password);
                if (!result) {
                    return Error.Validation(description: "Falha ao logar, verifique os seus dados");
                }

                var userInfo = await _authentication.GetUserInfoAsync(request.Email);
                if (userInfo == null) {
                    return Error.Validation(description: "Dados do usuário não encontrados");
                }

                var userToken = GenerateToken.GenerateUserToken(userInfo, _configuration);

                LoginCommandResult loginResult =
                    _mapper.Map<LoginCommandResult>(userToken);

                return loginResult;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
