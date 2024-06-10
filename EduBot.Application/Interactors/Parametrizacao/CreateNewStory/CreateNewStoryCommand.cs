using MediatR;

namespace EduBot.Application.Interactors.Parametrizacao.CreateNewStory {
    public record CreateNewStoryCommand(string TituloPergunta, List<string> Perguntas, List<string> Respostas) : IRequest<ErrorOr<Unit>>;
}
