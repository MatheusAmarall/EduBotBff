﻿using EduBot.Domain.Entities;
using MongoDB.Driver;

namespace EduBot.Infrastructure.Persistence.Context;

public interface IMongoDbContext : IDisposable {

    IMongoCollection<Conversation> Conversations { get; }
    IMongoCollection<Matricula> Matriculas { get; }
    IMongoCollection<Parametrizacao> Parametrizacoes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    void AddCommand(Func<Task> func);
}
