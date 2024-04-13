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

    public void AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken) {
        Context.AddCommand(
            () => _collection.InsertManyAsync(entities, cancellationToken: cancellationToken)
        );
    }

    public void DeleteRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken) {
        string[] ids = entities.Select(x => x.Id).ToArray();
        Context.AddCommand(
            () => _collection.DeleteManyAsync(x => ids.Contains(x.Id), cancellationToken)
        );
    }

    public void UpdateRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken) {
        foreach (TEntity entity in entities) {
            Update(entity, cancellationToken);
        }
    }

    public void Delete(object id, CancellationToken cancellationToken) {
        Context.AddCommand(
            () => _collection.DeleteOneAsync(x => x.Id == (string)id, cancellationToken)
        );
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken) {
        return _collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken) {
        TEntity result = await _collection
            .Find(x => x.Id == (string)id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return result;
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
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}