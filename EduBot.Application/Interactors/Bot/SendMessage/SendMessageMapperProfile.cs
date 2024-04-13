using AutoMapper;
using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Interactors.Bot.SendMessage {
    public class SendMessageMapperProfile : Profile {
        public SendMessageMapperProfile() {
            CreateMap<SendMessageCommandResult, SendMessageResultDto>();
        }
    }
}