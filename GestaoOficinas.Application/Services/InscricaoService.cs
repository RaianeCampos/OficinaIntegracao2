using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class InscricaoService : IInscricaoService
    {
        private readonly IInscricaoRepository _repository;
        private readonly IMapper _mapper;

        public InscricaoService(IInscricaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<InscricaoViewModel> CreateAsync(CreateInscricaoDto dto)
        {
            var inscricao = _mapper.Map<Inscricao>(dto);
            await _repository.AddAsync(inscricao);
            return _mapper.Map<InscricaoViewModel>(inscricao);
        }

        public async Task DeleteAsync(int idAluno, int idTurma)
        {
            var inscricao = await _repository.GetByIdAsync(idAluno, idTurma);
            if (inscricao == null) throw new KeyNotFoundException("Inscrição não encontrada.");
            await _repository.DeleteAsync(idAluno, idTurma);
        }

        public async Task<IEnumerable<InscricaoViewModel>> GetAllAsync()
        {
            var inscricoes = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<InscricaoViewModel>>(inscricoes);
        }

        public async Task<InscricaoViewModel> GetByIdAsync(int idAluno, int idTurma)
        {
            var inscricao = await _repository.GetByIdAsync(idAluno, idTurma);
            return inscricao == null ? null : _mapper.Map<InscricaoViewModel>(inscricao);
        }

        public async Task UpdateAsync(int idAluno, int idTurma, UpdateInscricaoDto dto)
        {
            var inscricao = await _repository.GetByIdAsync(idAluno, idTurma);
            if (inscricao == null) throw new KeyNotFoundException("Inscrição não encontrada.");

            _mapper.Map(dto, inscricao);
            await _repository.UpdateAsync(inscricao);
        }
    }
}
