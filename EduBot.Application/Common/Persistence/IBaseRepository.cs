using EduBot.Domain.Entities;

namespace EduBot.Application.Common.Persistence;

public interface IBaseRepository<TEntity> : IDisposable
    where TEntity : Entity {
    void Add(TEntity entity, CancellationToken cancellationToken);

    void AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    void Update(TEntity entity, CancellationToken cancellationToken);

    void UpdateRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    void Delete(object id, CancellationToken cancellationToken);

    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken);
}
