using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Application.Mappers
{
    public class MappingProfile : Profile
    {
        //MAPEAR ENTIDADE ESCOLA
        public MappingProfile()
        {
            CreateMap<Escola, EscolaViewModel>();
            
            CreateMap<CreateEscolaDto, Escola>();

            CreateMap<UpdateEscolaDto, Escola>();
        }
    }
}