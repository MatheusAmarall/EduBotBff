using EduBot.Application.Common.Interfaces;
using EduBot.Domain.Entities;
using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Interactors.Register {
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<Unit>> {
        private readonly IAuthenticate _authentication;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterCommandHandler(IAuthenticate authentication, IUnitOfWork unitOfWork) {
            _authentication = authentication;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken) {
            try {
                if(request.Password != request.ConfirmPassword) {
                    return Error.Validation(description: "As senhas não coincidem");
                }

                List<Matricula>  matriculasCadastradas = await _unitOfWork.Matriculas.GetMatriculasByRole(request.IsAdmin);

                var matriculaUsuario = matriculasCadastradas.Find(m => m.MatriculaUsuario == request.Matricula);
                if(matriculaUsuario is null) {
                    return Unit.Value;
                }

                bool matriculaExistente = _authentication.VerificarMatriculaExistente(request.Matricula);
                if(matriculaExistente) {
                    return Error.Validation(description: "Já existe um usuário cadastrado com a matrícula");
                }

                string result = await _authentication.RegisterUser(new User(request.IsAdmin, request.Matricula, request.Email, request.Password, request.ConfirmPassword));

                if(result != "") {
                    return Error.Validation(description: result);
                }

                return Unit.Value;
            }
            catch(Exception ex) {
                return Error.Validation(description: ex.Message);
            }
            
        }
    }
}
