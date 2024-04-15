using AutoMapper;
using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Interactors.Auth.Login {
    public class LoginMapperProfile : Profile {
        public LoginMapperProfile() {
            CreateMap<UserTokenDto, LoginCommandResult>().ReverseMap();
        }
    }
}