using EduBot.Domain.Entities;
using EduBot.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EduBot.Infrastructure.Persistence.Context;

public sealed class MongoDbContext : IMongoDbContext {
    private readonly List<Func<Task>> _commands;
    private readonly IMongoDatabase _db;
    private readonly MongoClient _mongoClient;
    private readonly bool _useTransaction;

    public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings) {
        _commands = new List<Func<Task>>();
        _mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        _db = _mongoClient.GetDatabase("EduBot");
        _useTransaction = mongoDbSettings.Value.UseTransaction;
    }

    private IClientSessionHandle? Session { get; } = null;

    public IMongoCollection<Conversation> Conversations =>
        _db.GetCollection<Conversation>("conversations");

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
        IEnumerable<Task> commandTasks = _commands.Select(c => c());
        if (_useTransaction) {
            using (
                IClientSessionHandle? session = await _mongoClient
                    .StartSessionAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false)
            ) {
                session.StartTransaction();
                await Task.WhenAll(commandTasks).ConfigureAwait(false);
                await session.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
            }

            _commands.Clear();
            return _commands.Count;
        }

        await Task.WhenAll(commandTasks).ConfigureAwait(false);
        _commands.Clear();
        return _commands.Count;
    }

    public void AddCommand(Func<Task> func) {
        _commands.Add(func);
    }

    public void Dispose() {
        Session?.Dispose();
        GC.SuppressFinalize(this);
    }
}
