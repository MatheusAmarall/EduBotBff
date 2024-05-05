using AutoMapper;
using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Interactors.Bot.GetMessages {
    public class CreateNewStoryMapperProfile : Profile {
        public CreateNewStoryMapperProfile() {
            CreateMap<string, Response>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src));
        }
    }
}