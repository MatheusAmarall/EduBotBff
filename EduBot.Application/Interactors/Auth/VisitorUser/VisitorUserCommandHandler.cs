using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Application.Common.Util;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EduBot.Application.Interactors.Auth.VisitorUser {
    public class VisitorUserCommandHandler : IRequestHandler<VisitorUserCommand, ErrorOr<VisitorUserCommandResult>> {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public VisitorUserCommandHandler(IMapper mapper, IConfiguration configuration) {
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ErrorOr<VisitorUserCommandResult>> Handle(VisitorUserCommand request, CancellationToken cancellationToken) {
            try {
                var userInfo = new UserInfoResultDto("", "Visitante");
                var userToken = GenerateToken.GenerateUserToken(userInfo, _configuration);

                VisitorUserCommandResult loginResult =
                    _mapper.Map<VisitorUserCommandResult>(userToken);

                return loginResult;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }

        }
    }
}
