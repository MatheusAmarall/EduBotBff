using AutoMapper;
using EduBot.Domain.Entities;

namespace EduBot.Application.Interactors.Bot.GetMessages {
    public class GetMessagesMapperProfile : Profile {
        public GetMessagesMapperProfile() {
            CreateMap<GetMessagesQueryResult, ConversationSimplify>().ReverseMap();
        }
    }
}