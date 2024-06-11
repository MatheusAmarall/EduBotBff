using EduBot.Domain.Entities;
using MongoDB.Driver;

namespace EduBot.Infrastructure.Persistence.Context;

public interface IMongoDbContext : IDisposable {

    IMongoCollection<Conversation> Conversations { get; }
    IMongoCollection<Conversa> Conversas { get; }
    IMongoCollection<Matricula> Matriculas { get; }
    IMongoCollection<Parametrizacao> Parametrizacoes { get; }
    IMongoCollection<Funcionalidade> Funcionalidades { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    void AddCommand(Func<Task> func);
}
