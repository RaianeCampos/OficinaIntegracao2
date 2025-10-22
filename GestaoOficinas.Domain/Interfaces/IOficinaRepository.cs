using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IOficinaRepository
    {
        Task<Oficina> GetByIdAsync(int id);
        Task<IEnumerable<Oficina>> GetAllAsync();
        Task AddAsync(Oficina oficina);
        Task UpdateAsync(Oficina oficina);
        Task DeleteAsync(int id);
    }
}
