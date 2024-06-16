using AutoMapper;

namespace EduBot.Application.Interactors.Agendamento.RetornaAgendamentos;
public class RetornaAgendamentosMapperProfile : Profile {
    public RetornaAgendamentosMapperProfile() {
        CreateMap<Domain.Entities.Agendamento, RetornaAgendamentosQueryResult>()
            .ForMember(dest => dest.DataAgendamento, opt => 
                opt.MapFrom(src => src.DataAgendamento.ToString("dd/MM/yyyy"))); ;
    }
}