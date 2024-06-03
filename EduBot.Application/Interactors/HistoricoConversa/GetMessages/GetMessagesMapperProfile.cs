using AutoMapper;
using EduBot.Domain.Entities;

namespace EduBot.Application.Interactors.HistoricoConversa.GetMessages {
    public class GetMessagesMapperProfile : Profile {
        public GetMessagesMapperProfile() {
            CreateMap<GetMessagesQueryResult, Conversa>().ReverseMap();
            CreateMap<MessageResult, Message>().ReverseMap();
        }
    }
}