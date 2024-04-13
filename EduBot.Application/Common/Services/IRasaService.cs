using EduBot.Application.Common.DTOs;
using EduBot.Application.Interactors.Bot.SendMessage;
using Refit;

namespace EduBot.Application.Common.Services {
    public interface IRasaService {
        [Post("/webhooks/rest/webhook")]
        Task<IEnumerable<SendMessageResultDto>> SendMessageAsync([Body] SendMessageCommand request);
    }
}
