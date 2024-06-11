namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable {
    IConversationsRepository Conversations { get; }
    IMatriculasRepository Matriculas { get; }
    IConversasRepository Conversas { get; }
    IParametrizacoesRepository Parametrizacoes { get; }
    IFuncionalidadesRepository Funcionalidades { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
