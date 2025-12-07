using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _repository;
        private readonly IMapper _mapper;

        public AlunoService(IAlunoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AlunoViewModel> CreateAsync(CreateAlunoDto dto)
        {
            var aluno = _mapper.Map<Aluno>(dto);
            aluno.NascimentoAluno = DateTime.SpecifyKind(aluno.NascimentoAluno, DateTimeKind.Utc);

            await _repository.AddAsync(aluno);
            return _mapper.Map<AlunoViewModel>(aluno);
        }

        public async Task DeleteAsync(int id)
        {
            var aluno = await _repository.GetByIdAsync(id);
            if (aluno == null) throw new KeyNotFoundException("Aluno não encontrado.");
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AlunoViewModel>> GetAllAsync()
        {
            var alunos = await _repository.GetAllWithTurmaAsync();
            return _mapper.Map<IEnumerable<AlunoViewModel>>(alunos);
        }

        public async Task<AlunoViewModel> GetByIdAsync(int id)
        {
            var aluno = await _repository.GetByIdAsync(id);
            return aluno == null ? null : _mapper.Map<AlunoViewModel>(aluno);
        }

        public async Task UpdateAsync(int id, UpdateAlunoDto dto)
        {
            var aluno = await _repository.GetByIdAsync(id);
            if (aluno == null) throw new KeyNotFoundException("Aluno não encontrado.");

            _mapper.Map(dto, aluno);

            aluno.NascimentoAluno = DateTime.SpecifyKind(aluno.NascimentoAluno, DateTimeKind.Utc);

            await _repository.UpdateAsync(aluno);
        }
    }
}
