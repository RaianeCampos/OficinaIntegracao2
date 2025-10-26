using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class ChamadaService : IChamadaService
    {
        private readonly IChamadaRepository _repository;
        private readonly IMapper _mapper;

        public ChamadaService(IChamadaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ChamadaViewModel> CreateAsync(CreateChamadaDto dto)
        {
            var chamada = _mapper.Map<Chamada>(dto);
            await _repository.AddAsync(chamada);
            return _mapper.Map<ChamadaViewModel>(chamada);
        }

        public async Task DeleteAsync(int id)
        {
            var chamada = await _repository.GetByIdAsync(id);
            if (chamada == null) throw new KeyNotFoundException("Chamada não encontrada.");
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ChamadaViewModel>> GetAllAsync()
        {
            var chamadas = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ChamadaViewModel>>(chamadas);
        }

        public async Task<ChamadaViewModel> GetByIdAsync(int id)
        {
            var chamada = await _repository.GetByIdAsync(id);
            return chamada == null ? null : _mapper.Map<ChamadaViewModel>(chamada);
        }
    }
}
