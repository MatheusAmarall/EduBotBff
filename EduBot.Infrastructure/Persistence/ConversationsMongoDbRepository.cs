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

    public async Task<IEnumerable<ConversationSimplify?>> GetAllEventsAsync() {
        var result = await Context
            .Conversations.AsQueryable().Select(c => new {
                c.sender_id,
                c.events
            }).ToListAsync();

        List<ConversationSimplify> eventos = new();

        result.ForEach(c => {
            var eventObjects = ConvertEvents(c.events);
            var evento = new ConversationSimplify {
                SenderId = c.sender_id,
                Events = eventObjects
            };

            eventos.Add(evento);
        });

        return eventos;
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