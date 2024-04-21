using EduBot.Application.Common.Persistence;
using EduBot.Domain.Entities;

namespace Vips.EstoqueBase.Application.Common.Interfaces.Persistence;

public interface IConversationsRepository : IBaseRepository<Conversation> {
    Task<ConversationSimplify> GetByEmailAsync(string email);
}
