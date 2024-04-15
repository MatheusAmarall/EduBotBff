using AutoMapper;
using EduBot.Application.Common.DTOs;

namespace EduBot.Application.Interactors.Auth.VisitorUser {
    public class VisitorUserMapperProfile : Profile {
        public VisitorUserMapperProfile() {
            CreateMap<UserTokenDto, VisitorUserCommandResult>().ReverseMap();
        }
    }
}