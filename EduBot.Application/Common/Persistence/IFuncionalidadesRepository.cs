using EduBot.Application.Common.Persistence;
using EduBot.Domain.Entities;

namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IFuncionalidadesRepository : IBaseRepository<Funcionalidade> {
    Task<Funcionalidade?> GetFuncionalidadeByNome(string nomeFuncionalidade);
}
