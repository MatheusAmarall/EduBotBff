namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable {
    IConversationsRepository Conversations { get; }
    IMatriculasRepository Matriculas { get; }
    IParametrizacoesRepository Parametrizacoes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
