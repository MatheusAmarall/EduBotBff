namespace EduBot.Infrastructure.Persistence.Context;

public interface IMongoDbContext : IDisposable {

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    void AddCommand(Func<Task> func);
}
