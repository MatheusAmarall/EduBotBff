using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence;
using EduBot.Infrastructure.Persistence.Context;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

public sealed class ConversasMongoDbRepository
    : MongoDbRepository<Conversa>,
        IConversasRepository {
    public ConversasMongoDbRepository(IMongoDbContext context)
        : base(context, context.Conversas) { }

    public async Task<Conversa?> GetConversaByNome(string nomeUsuario) {
        var result = await Context
            .Conversas.AsQueryable()
            .FirstOrDefaultAsync(c =>
                c.NomeUsuario == nomeUsuario
            );

        return result;
    }

    public async Task<IEnumerable<Conversa>> GetConversasUsuarios() {
        var result = await Context
            .Conversas.AsQueryable()
            .Where(c => c.Role == "User").ToListAsync();

        return result;
    }
}