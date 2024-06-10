using EduBot.Domain.Entities;

namespace EduBot.Application.Common.Persistence;

public interface IBaseRepository<TEntity> : IDisposable
    where TEntity : Entity {
    void Add(TEntity entity, CancellationToken cancellationToken);

    void Update(TEntity entity, CancellationToken cancellationToken);

    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
}
