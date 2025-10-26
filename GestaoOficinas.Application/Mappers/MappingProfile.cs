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
            CreateMap<Oficina, OficinaViewModel>();
            CreateMap<CreateOficinaDto, Oficina>();
            CreateMap<UpdateOficinaDto, Oficina>();

            // Turma
            CreateMap<Turma, TurmaViewModel>();
            CreateMap<CreateTurmaDto, Turma>();
            CreateMap<UpdateTurmaDto, Turma>();

            // Inscricao
            CreateMap<Inscricao, InscricaoViewModel>();
            CreateMap<CreateInscricaoDto, Inscricao>();
            CreateMap<UpdateInscricaoDto, Inscricao>();

            // Chamada
            CreateMap<Chamada, ChamadaViewModel>();
            CreateMap<CreateChamadaDto, Chamada>();

            // Documento
            CreateMap<Documento, DocumentoViewModel>();
            CreateMap<CreateDocumentoDto, Documento>();
            CreateMap<UpdateDocumentoDto, Documento>();
        }
    }
}
