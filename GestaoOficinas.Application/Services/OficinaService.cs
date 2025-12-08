using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class OficinaService : IOficinaService
    {
        private readonly IOficinaRepository _repository;
        private readonly IMapper _mapper;

        public OficinaService(IOficinaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OficinaViewModel> CreateAsync(CreateOficinaDto dto)
        {
            var oficina = _mapper.Map<Oficina>(dto);

            oficina.DataOficina = DateTime.SpecifyKind(oficina.DataOficina, DateTimeKind.Utc);

            if (string.IsNullOrEmpty(oficina.StatusOficina))
            {
                oficina.StatusOficina = "Em Andamento";
            }

            await _repository.AddAsync(oficina);
            return _mapper.Map<OficinaViewModel>(oficina);
        }

        public async Task DeleteAsync(int id)
        {
            var oficina = await _repository.GetByIdAsync(id);
            if (oficina == null) throw new KeyNotFoundException("Oficina não encontrada.");
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OficinaViewModel>> GetAllAsync()
        {
            var oficinas = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OficinaViewModel>>(oficinas);
        }

        public async Task<OficinaViewModel> GetByIdAsync(int id)
        {
            var oficina = await _repository.GetByIdAsync(id);
            return oficina == null ? null : _mapper.Map<OficinaViewModel>(oficina);
        }

        public async Task UpdateAsync(int id, UpdateOficinaDto dto)
        {
            var oficina = await _repository.GetByIdAsync(id);
            if (oficina == null) throw new KeyNotFoundException("Oficina não encontrada.");

            _mapper.Map(dto, oficina);

            oficina.DataOficina = DateTime.SpecifyKind(oficina.DataOficina, DateTimeKind.Utc);

            await _repository.UpdateAsync(oficina);
        }
    }
}
