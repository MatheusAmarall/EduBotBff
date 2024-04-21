using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence;
using EduBot.Infrastructure.Persistence.Context;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

public sealed class ConversationsMongoDbRepository
    : MongoDbRepository<Conversation>,
        IConversationsRepository {
    public ConversationsMongoDbRepository(IMongoDbContext context)
        : base(context, context.Conversations) { }

    public async Task<ConversationSimplify> GetByEmailAsync(string email) {
        var result = await Context
            .Conversations.AsQueryable()
            .Where(c =>
                c.sender_id == email
            )
            .Select(c => new ConversationSimplify {
                SenderId = c.sender_id,
                Events = c.events
            })
            .FirstOrDefaultAsync();

        return result;
    }

    
}