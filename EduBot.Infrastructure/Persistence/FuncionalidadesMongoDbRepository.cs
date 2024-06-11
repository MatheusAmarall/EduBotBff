using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence;
using EduBot.Infrastructure.Persistence.Context;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

public sealed class FuncionalidadesMongoDbRepository
    : MongoDbRepository<Funcionalidade>,
        IFuncionalidadesRepository {
    public FuncionalidadesMongoDbRepository(IMongoDbContext context)
        : base(context, context.Funcionalidades) { }

    public async Task<Funcionalidade?> GetFuncionalidadeByNome(string nomeFuncionalidade) {
        var result = await Context
            .Funcionalidades.AsQueryable()
            .FirstOrDefaultAsync(c =>
                c.NomeFuncionalidade == nomeFuncionalidade
            );

        return result;
    }
}