using EduBot.Application.Common.DTOs;
using EduBot.Application.Interactors.Bot.SendMessage;
using EduBot.Domain.Entities;
using Refit;

namespace EduBot.Application.Common.Services {
    public interface IRasaService {
        [Post("/webhooks/rest/webhook")]
        Task<IEnumerable<SendMessageResultDto>> SendMessageAsync([Body] SendMessageCommand request);

        [Headers("Content-Type: application/yaml")]
        [Post("/model/train")]
        Task<HttpResponseMessage> TrainRasaModelAsync([Query] string token, [Body] string yaml);

        [Put("/model")]
        Task<Stream> ReplaceLoadedModelAsync([Query] string token, [Body] ReplaceLoadedModelDto request);
    }
}
