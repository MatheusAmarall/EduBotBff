using MediatR;

namespace EduBot.Application.Interactors.HistoricoConversa.GetMessages {
    public record GetMessagesQuery(string Email) : IRequest<ErrorOr<GetMessagesQueryResult>>;
}
