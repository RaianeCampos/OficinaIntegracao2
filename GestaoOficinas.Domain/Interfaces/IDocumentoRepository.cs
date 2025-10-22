using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IDocumentoRepository
    {
        Task<Documento> GetByIdAsync(int id);
        Task<IEnumerable<Documento>> GetAllAsync();
        Task AddAsync(Documento documento);
        Task UpdateAsync(Documento documento);
        Task DeleteAsync(int id);
    }
}
