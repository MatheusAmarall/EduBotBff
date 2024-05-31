using AutoMapper;
using EduBot.Domain.Entities;

namespace EduBot.Application.Interactors.HistoricoConversa.GetAllMessages {
    public class GetAllMessagesMapperProfile : Profile {
        public GetAllMessagesMapperProfile() {
            CreateMap<GetAllMessagesQueryResult, Conversa>().ReverseMap();
            CreateMap<MessageResult, Message>().ReverseMap();
        }
    }
}