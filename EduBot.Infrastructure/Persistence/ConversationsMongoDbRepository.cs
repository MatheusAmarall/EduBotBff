using EduBot.Domain.Entities;
using EduBot.Infrastructure.Persistence;
using EduBot.Infrastructure.Persistence.Context;
using System.Text.Json;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

namespace Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

public sealed class ConversationsMongoDbRepository
    : MongoDbRepository<Conversation>,
        IConversationsRepository {
    public ConversationsMongoDbRepository(IMongoDbContext context)
        : base(context, context.Conversations) { }

    public async Task<ConversationSimplify?> GetByEmailAsync(string email) {
        var result = await Context
            .Conversations.AsQueryable()
            .Where(c => c.sender_id == email)
            .Select(c => new {
                c.sender_id,
                c.events
            })
            .FirstOrDefaultAsync();

        if(result == null) {
            return null;
        }

        var eventObjects = ConvertEvents(result.events);

        return new ConversationSimplify {
            SenderId = result.sender_id,
            Events = eventObjects
        };
    }

    private List<EventObject> ConvertEvents(List<object> events) {
        List<EventObject> eventList = new List<EventObject>();

        foreach (var ev in events) {
            var evJson = JsonSerializer.Serialize(ev);
            var eventObject = JsonSerializer.Deserialize<EventObject>(evJson);
            if (eventObject != null) {
                eventList.Add(eventObject);
            }
        }

        return eventList;
    }
}