﻿using EduBot.Infrastructure.Persistence.Context;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork {
    private readonly IMongoDbContext _context;
    private bool _disposedValue;

    public UnitOfWork(
        IMongoDbContext context,
        IConversationsRepository conversations,
        IMatriculasRepository matriculas,
        IParametrizacoesRepository parametrizacoes
    ) {
        _context = context;
        Conversations = conversations;
        Matriculas = matriculas;
        Parametrizacoes = parametrizacoes;
    }

    public IConversationsRepository Conversations { get; }
    public IMatriculasRepository Matriculas { get; }
    public IParametrizacoesRepository Parametrizacoes { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) {
        return _context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                _context.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}