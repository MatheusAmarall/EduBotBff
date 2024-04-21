using EduBot.Application.Common.Persistence;
using EduBot.Domain.Entities;

namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IMatriculasRepository : IBaseRepository<Matricula> {
    Task<List<Matricula>> GetMatriculasByRole(bool admin);
}
