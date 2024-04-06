using EduBot.Application.Interactors.Bot.SendMessage;
using Refit;

namespace EduBot.Application.Common.Services {
    public interface IRasaService {
        [Post("/webhooks/rest/webhook")]
        Task<IEnumerable<SendMessageCommandResult>> SendMessageAsync([Body] SendMessageCommand request);
    }
}
