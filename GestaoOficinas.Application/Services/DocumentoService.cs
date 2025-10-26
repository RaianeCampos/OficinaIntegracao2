using AutoMapper;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Application.Interfaces;
using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;

namespace GestaoOficinas.Application.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IDocumentoRepository _repository;
        private readonly IMapper _mapper;

        public DocumentoService(IDocumentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DocumentoViewModel> CreateAsync(CreateDocumentoDto dto)
        {
            var documento = _mapper.Map<Documento>(dto);
            await _repository.AddAsync(documento);
            return _mapper.Map<DocumentoViewModel>(documento);
        }

        public async Task DeleteAsync(int id)
        {
            var documento = await _repository.GetByIdAsync(id);
            if (documento == null) throw new KeyNotFoundException("Documento não encontrado.");
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DocumentoViewModel>> GetAllAsync()
        {
            var documentos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DocumentoViewModel>>(documentos);
        }

        public async Task<DocumentoViewModel> GetByIdAsync(int id)
        {
            var documento = await _repository.GetByIdAsync(id);
            return documento == null ? null : _mapper.Map<DocumentoViewModel>(documento);
        }

        public async Task UpdateAsync(int id, UpdateDocumentoDto dto)
        {
            var documento = await _repository.GetByIdAsync(id);
            if (documento == null) throw new KeyNotFoundException("Documento não encontrado.");

            _mapper.Map(dto, documento);
            await _repository.UpdateAsync(documento);
        }
    }
}
