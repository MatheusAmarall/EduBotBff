using MediatR;

namespace EduBot.Application.Interactors.HistoricoConversa.GetAllMessages {
    public record GetAllMessagesQuery() : IRequest<ErrorOr<IEnumerable<GetAllMessagesQueryResult>>>;
}
