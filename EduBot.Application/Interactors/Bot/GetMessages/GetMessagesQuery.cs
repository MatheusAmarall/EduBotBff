using MediatR;

namespace EduBot.Application.Interactors.Bot.GetMessages {
    public record GetMessagesQuery(string Email) : IRequest<ErrorOr<GetMessagesQueryResult>>;
}
