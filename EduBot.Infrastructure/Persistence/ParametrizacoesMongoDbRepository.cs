using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence;
using EduBot.Infrastructure.Persistence.Context;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

public sealed class ParametrizacoesMongoDbRepository
    : MongoDbRepository<Parametrizacao>,
        IParametrizacoesRepository {
    public ParametrizacoesMongoDbRepository(IMongoDbContext context)
        : base(context, context.Parametrizacoes) { }
}