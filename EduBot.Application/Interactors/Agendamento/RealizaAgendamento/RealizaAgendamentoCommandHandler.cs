using MediatR;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace EduBot.Application.Interactors.Agendamento.RealizaAgendamento {
    public class RealizaAgendamentoCommandHandler : IRequestHandler<RealizaAgendamentoCommand,
        ErrorOr<RealizaAgendamentoCommandResult>> {
        private readonly IUnitOfWork _unitOfWork;
        public RealizaAgendamentoCommandHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<RealizaAgendamentoCommandResult>> Handle(RealizaAgendamentoCommand request, CancellationToken cancellationToken) {
            try {
                var usuario = await _unitOfWork.Conversas.GetConversaByNome(request.Email);
                if(usuario is null || usuario.Role != "User") {
                    return Error.Validation(description: "Você não tem a permissão necessária para realizar agendamentos");
                }

                var ultimoAgendamentoUsuario = await _unitOfWork.Agendamentos.GetUltimoAgendamentoByNome(request.Email);

                if(ultimoAgendamentoUsuario is not null && ultimoAgendamentoUsuario.DataAgendamento > DateTime.Now) {
                    return Error.Validation(description: "Você já tem um agendamento futuro");
                }

                var novoAgendamento = new Domain.Entities.Agendamento() {
                    NomeUsuario = request.Email,
                    DataAgendamento = DateTime.Now.AddDays(1)
                };

                _unitOfWork.Agendamentos.Add(novoAgendamento, CancellationToken.None);

                await _unitOfWork.SaveChangesAsync(new CancellationToken());

                var agendamentoResult = new RealizaAgendamentoCommandResult() {
                    DataAgendamento = novoAgendamento.DataAgendamento.ToString("dd/MM/yyyy")
                };

                return agendamentoResult;
            }
            catch (Exception ex) {
                return Error.Validation(description: ex.Message);
            }
        }
    }
}
