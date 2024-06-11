using EduBot.Application.Common.Persistence;
using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence.Context;
using MongoDB.Driver;

namespace EduBot.Infrastructure.Persistence;

public abstract class MongoDbRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : Entity {
    private readonly IMongoCollection<TEntity> _collection;
    protected IMongoDbContext Context { get; }
    private bool _disposedValue;

    protected MongoDbRepository(IMongoDbContext context, IMongoCollection<TEntity> collection) {
        Context = context;
        _collection = collection;
    }

    public void Add(TEntity entity, CancellationToken cancellationToken) {
        Context.AddCommand(
            () => _collection.InsertOneAsync(entity, cancellationToken: cancellationToken)
        );
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken) {
        return _collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public void Update(TEntity entity, CancellationToken cancellationToken) {
        Context.AddCommand(
            () =>
                _collection.ReplaceOneAsync(
                    x => x.Id == entity.Id,
                    entity,
                    cancellationToken: cancellationToken
                )
        );
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            _disposedValue = true;
        }
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}