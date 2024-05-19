using EduBot.Application.Common.Persistence;
using EduBot.Domain.Entities;

namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IConversasRepository : IBaseRepository<Conversa> {
    Task<Conversa?> GetConversaByNome(string nomeUsuario);
}
