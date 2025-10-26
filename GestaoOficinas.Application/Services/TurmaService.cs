using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _repository;
        private readonly IMapper _mapper;

        public TurmaService(ITurmaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TurmaViewModel> CreateAsync(CreateTurmaDto dto)
        {
            var turma = _mapper.Map<Turma>(dto);
            await _repository.AddAsync(turma);
            return _mapper.Map<TurmaViewModel>(turma);
        }

        public async Task DeleteAsync(int id)
        {
            var turma = await _repository.GetByIdAsync(id);
            if (turma == null) throw new KeyNotFoundException("Turma não encontrada.");
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TurmaViewModel>> GetAllAsync()
        {
            var turmas = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TurmaViewModel>>(turmas);
        }

        public async Task<TurmaViewModel> GetByIdAsync(int id)
        {
            var turma = await _repository.GetByIdAsync(id);
            return turma == null ? null : _mapper.Map<TurmaViewModel>(turma);
        }

        public async Task UpdateAsync(int id, UpdateTurmaDto dto)
        {
            var turma = await _repository.GetByIdAsync(id);
            if (turma == null) throw new KeyNotFoundException("Turma não encontrada.");

            _mapper.Map(dto, turma);
            await _repository.UpdateAsync(turma);
        }
    }
}
