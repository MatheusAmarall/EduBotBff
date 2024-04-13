using MediatR;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public class SendMessageCommand : IRequest<ErrorOr<IEnumerable<SendMessageCommandResult>>> {
        public string Sender { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
