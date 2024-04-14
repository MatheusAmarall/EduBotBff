using MediatR;

namespace EduBot.Application.Interactors.Auth.VisitorUser {
    public record VisitorUserCommand() : IRequest<ErrorOr<VisitorUserCommandResult>>;
}
