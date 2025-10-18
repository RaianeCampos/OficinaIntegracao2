using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class EscolaService : IEscolaService
    {
        private readonly IEscolaRepository _repository;
        private readonly IMapper _mapper;

        public EscolaService(IEscolaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EscolaViewModel> CreateAsync(CreateEscolaDto dto)
        {
            // Regra de Negócio: Não permitir CNPJ duplicado
            if (await _repository.CnpjExistsAsync(dto.CnpjEscola))
            {
                throw new Exception("Já existe uma escola com este CNPJ.");
            }

            var escola = _mapper.Map<Escola>(dto);
            await _repository.AddAsync(escola);

            return _mapper.Map<EscolaViewModel>(escola);
        }

        public async Task DeleteAsync(int id)
        {
            var escola = await _repository.GetByIdAsync(id);
            if (escola == null)
            {
                throw new KeyNotFoundException("Escola não encontrada.");
            }
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EscolaViewModel>> GetAllAsync()
        {
            var escolas = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<EscolaViewModel>>(escolas);
        }

        public async Task<EscolaViewModel> GetByIdAsync(int id)
        {
            var escola = await _repository.GetByIdAsync(id);
            if (escola == null)
            {
                return null;
            }
            return _mapper.Map<EscolaViewModel>(escola);
        }

        public async Task UpdateAsync(int id, UpdateEscolaDto dto)
        {
            var escola = await _repository.GetByIdAsync(id);
            if (escola == null)
            {
                throw new KeyNotFoundException("Escola não encontrada.");
            }

            // Mapeia o DTO para a entidade existente (atualização)
            _mapper.Map(dto, escola);
            await _repository.UpdateAsync(escola);
        }
    }
}