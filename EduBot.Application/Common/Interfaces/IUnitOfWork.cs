namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable {
    IConversationsRepository Conversations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
