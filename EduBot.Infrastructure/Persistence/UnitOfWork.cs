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
        IParametrizacoesRepository parametrizacoes,
        IConversasRepository conversas,
        IFuncionalidadesRepository funcionalidades,
        IAgendamentosRepository agendamentos
    ) {
        _context = context;
        Conversations = conversations;
        Matriculas = matriculas;
        Parametrizacoes = parametrizacoes;
        Conversas = conversas;
        Funcionalidades = funcionalidades;
        Agendamentos = agendamentos;
    }

    public IConversationsRepository Conversations { get; }
    public IMatriculasRepository Matriculas { get; }
    public IParametrizacoesRepository Parametrizacoes { get; }
    public IConversasRepository Conversas { get; }
    public IFuncionalidadesRepository Funcionalidades { get; }
    public IAgendamentosRepository Agendamentos { get; }

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