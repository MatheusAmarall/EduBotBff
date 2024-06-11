using AutoMapper;
using EduBot.Domain.Entities;

namespace EduBot.Application.Interactors.Parametrizacao.GetFuncionalidadesParametrizadas {
    public class GetFuncionalidadesParametrizadasMapperProfile : Profile {
        public GetFuncionalidadesParametrizadasMapperProfile() {
            CreateMap<GetFuncionalidadesParametrizadasQueryResult, Funcionalidade>().ReverseMap();
        }
    }
}