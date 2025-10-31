using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Escola
            CreateMap<Escola, EscolaViewModel>();
            CreateMap<CreateEscolaDto, Escola>();
            CreateMap<UpdateEscolaDto, Escola>();

            // Professor
            CreateMap<Professor, ProfessorViewModel>();
            CreateMap<CreateProfessorDto, Professor>();
            CreateMap<UpdateProfessorDto, Professor>();

            // Aluno
            CreateMap<Aluno, AlunoViewModel>();
            CreateMap<CreateAlunoDto, Aluno>();
            CreateMap<UpdateAlunoDto, Aluno>();

            // Oficina
            CreateMap<Oficina, OficinaViewModel>()
                .ForMember(dest => dest.NomeProfessorResponsavel,
                           opt => opt.MapFrom(src => src.ProfessorResponsavel.NomeProfessor));
            CreateMap<CreateOficinaDto, Oficina>();
            CreateMap<UpdateOficinaDto, Oficina>();

            // Turma
            CreateMap<Turma, TurmaViewModel>()
                .ForMember(dest => dest.NomeOficina,
                           opt => opt.MapFrom(src => src.Oficina.NomeOficina));
            CreateMap<CreateTurmaDto, Turma>();
            CreateMap<UpdateTurmaDto, Turma>();

            // Inscricao
            CreateMap<Inscricao, InscricaoViewModel>()
                .ForMember(dest => dest.NomeAluno,
                           opt => opt.MapFrom(src => src.Aluno.NomeAluno))
                .ForMember(dest => dest.NomeTurma,
                           opt => opt.MapFrom(src => src.Turma.NomeTurma));
            CreateMap<CreateInscricaoDto, Inscricao>();
            CreateMap<UpdateInscricaoDto, Inscricao>();

            // Chamada
            CreateMap<Chamada, ChamadaViewModel>()
                .ForMember(dest => dest.NomeTurma,
                           opt => opt.MapFrom(src => src.Turma.NomeTurma));
            CreateMap<CreateChamadaDto, Chamada>();

            // Documento
            CreateMap<Documento, DocumentoViewModel>()
                .ForMember(dest => dest.NomeOficina,
                           opt => opt.MapFrom(src => src.Oficina.NomeOficina));
            CreateMap<CreateDocumentoDto, Documento>();
            CreateMap<UpdateDocumentoDto, Documento>();

            // Presenca
            CreateMap<Presenca, PresencaDto>()
                .ForMember(dest => dest.NomeAluno,
                           opt => opt.MapFrom(src => src.Aluno.NomeAluno));

            // OficinaTutor
            CreateMap<OficinaTutorDto, OficinaTutor>();
        }
    }
}

