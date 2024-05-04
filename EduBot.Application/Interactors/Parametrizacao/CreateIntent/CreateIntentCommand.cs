using MediatR;

namespace EduBot.Application.Interactors.Bot.CreateIntent {
    public record CreateIntentCommand() : IRequest<ErrorOr<CreateIntentCommandResult>>;
}
