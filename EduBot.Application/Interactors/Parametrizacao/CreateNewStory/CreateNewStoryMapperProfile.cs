using AutoMapper;
using EduBot.Application.Common.DTOs;
using EduBot.Domain.Entities;

namespace EduBot.Application.Interactors.Parametrizacao.CreateNewStory {
    public class CreateNewStoryMapperProfile : Profile {
        public CreateNewStoryMapperProfile() {
            CreateMap<string, ResponseDto>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src));

            CreateMap<Domain.Entities.Parametrizacao, ParametrizacaoDto>().ReverseMap();
            CreateMap<Slot, SlotDto>().ReverseMap();
            CreateMap<Form, FormDto>().ReverseMap();
            CreateMap<SessionConfig, SessionConfigDto>().ReverseMap();
            CreateMap<Mapping, MappingDto>().ReverseMap();
            CreateMap<Condition, ConditionDto>().ReverseMap();
            CreateMap<Response, ResponseDto>().ReverseMap();
        }
    }
}