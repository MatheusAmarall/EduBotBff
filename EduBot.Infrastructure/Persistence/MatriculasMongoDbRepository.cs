using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence;
using EduBot.Infrastructure.Persistence.Context;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

public sealed class MatriculasMongoDbRepository
    : MongoDbRepository<Matricula>,
        IMatriculasRepository {
    public MatriculasMongoDbRepository(IMongoDbContext context)
        : base(context, context.Matriculas) { }

    public async Task<List<Matricula>> GetMatriculasByRole(bool admin) {
        var result = await Context
            .Matriculas.AsQueryable()
            .Where(m =>
                m.IsAdmin == admin
            )
            .ToListAsync();

        return result;
    }
}