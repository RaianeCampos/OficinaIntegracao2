using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _repository;
        private readonly IMapper _mapper;

        public ProfessorService(IProfessorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProfessorViewModel> CreateAsync(CreateProfessorDto dto)
        {
            var professor = _mapper.Map<Professor>(dto);
            await _repository.AddAsync(professor);
            return _mapper.Map<ProfessorViewModel>(professor);
        }

        public async Task DeleteAsync(int id)
        {
            var professor = await _repository.GetByIdAsync(id);
            if (professor == null) throw new KeyNotFoundException("Professor não encontrado.");
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProfessorViewModel>> GetAllAsync()
        {
            var professores = await _repository.GetAllWithEscolaAsync();
            return _mapper.Map<IEnumerable<ProfessorViewModel>>(professores);
        }

        public async Task<ProfessorViewModel> GetByIdAsync(int id)
        {
            var professor = await _repository.GetByIdAsync(id);
            return professor == null ? null : _mapper.Map<ProfessorViewModel>(professor);
        }

        public async Task UpdateAsync(int id, UpdateProfessorDto dto)
        {
            var professor = await _repository.GetByIdAsync(id);
            if (professor == null) throw new KeyNotFoundException("Professor não encontrado.");

            _mapper.Map(dto, professor);
            await _repository.UpdateAsync(professor);
        }
    }
}
