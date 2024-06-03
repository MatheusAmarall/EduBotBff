using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Domain.Entities;

namespace EduBot.Application.Interactors.HistoricoConversa.GetAllMessages {
    public class ApplicationMapperProfile : Profile {
        public ApplicationMapperProfile() {
            CreateMap<MessageHistoryDto, Conversa>().ReverseMap();
            CreateMap<MessageResultHistoryDto, Message>().ReverseMap();
        }
    }
}