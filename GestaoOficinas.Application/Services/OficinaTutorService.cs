using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Application.Services
{
    public class OficinaTutorService : IOficinaTutorService
    {
        private readonly IOficinaTutorRepository _oficinaTutorRepository;
        private readonly IOficinaRepository _oficinaRepository;
        private readonly IMapper _mapper;

        public OficinaTutorService(IOficinaTutorRepository oficinaTutorRepository, IOficinaRepository oficinaRepository, IMapper mapper)
        {
            _oficinaTutorRepository = oficinaTutorRepository;
            _oficinaRepository = oficinaRepository;
            _mapper = mapper;
        }

        public async Task AdicionarTutorAsync(OficinaTutorDto dto)
        {
            var oficinaTutor = _mapper.Map<OficinaTutor>(dto);
            await _oficinaTutorRepository.AddAsync(oficinaTutor);
        }

        public async Task<OficinaComTutoresDto> GetOficinaComTutoresAsync(int idOficina)
        {
            var oficina = await _oficinaRepository.GetByIdAsync(idOficina);
            if (oficina == null)
            {
                throw new KeyNotFoundException("Oficina não encontrada.");
            }

            var tutores = await _oficinaTutorRepository.GetTutoresByOficinaAsync(idOficina);

            return new OficinaComTutoresDto
            {
                Oficina = _mapper.Map<OficinaViewModel>(oficina),
                Tutores = _mapper.Map<IEnumerable<ProfessorViewModel>>(tutores)
            };
        }

        public async Task RemoverTutorAsync(OficinaTutorDto dto)
        {
            await _oficinaTutorRepository.DeleteAsync(dto.IdOficina, dto.IdProfessor);
        }
    }
}
