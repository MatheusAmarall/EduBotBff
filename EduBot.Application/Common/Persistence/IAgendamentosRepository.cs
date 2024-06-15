using EduBot.Application.Common.Persistence;
using EduBot.Domain.Entities;

namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IAgendamentosRepository : IBaseRepository<Agendamento> {
    Task<Agendamento?> GetUltimoAgendamentoByNome(string nomeUsuario);
}
