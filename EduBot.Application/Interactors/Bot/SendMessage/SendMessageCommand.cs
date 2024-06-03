using MediatR;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public record SendMessageCommand(string NomeUsuario, string Mensagem, string Role, string Sender) :
        IRequest<ErrorOr<IEnumerable<SendMessageCommandResult>>>;
}
