using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence;
using EduBot.Infrastructure.Persistence.Context;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

public sealed class AgendamentosMongoDbRepository
    : MongoDbRepository<Agendamento>,
        IAgendamentosRepository {
    public AgendamentosMongoDbRepository(IMongoDbContext context)
        : base(context, context.Agendamentos) { }

    public async Task<Agendamento?> GetUltimoAgendamentoByNome(string nomeUsuario) {
        var result = await Context
            .Agendamentos.AsQueryable()
            .Where(c => c.NomeUsuario == nomeUsuario)
            .OrderByDescending(c => c.DataAgendamento)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<List<Agendamento>> GetProximosAgendamentos() {
        var result = await Context
            .Agendamentos.AsQueryable()
            .Where(c => c.DataAgendamento >= DateTime.Now)
            .OrderBy(c => c.DataAgendamento)
            .ToListAsync();

        return result;
    }
}