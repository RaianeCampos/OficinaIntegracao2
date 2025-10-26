using GestaoOficinas.Application.DTOs;

namespace GestaoOficinas.Application.Interfaces
{
    public interface IDocumentoService
    {
        Task<DocumentoViewModel> GetByIdAsync(int id);
        Task<IEnumerable<DocumentoViewModel>> GetAllAsync();
        Task<DocumentoViewModel> CreateAsync(CreateDocumentoDto dto);
        Task UpdateAsync(int id, UpdateDocumentoDto dto);
        Task DeleteAsync(int id);
    }
}
