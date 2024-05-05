using MediatR;

namespace EduBot.Application.Interactors.Bot.CreateNewStory {
    public record CreateNewStoryCommand(string TituloPergunta, List<string> Perguntas, List<string> Respostas) : IRequest<ErrorOr<Unit>>;
}
