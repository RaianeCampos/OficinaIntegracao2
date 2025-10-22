using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IChamadaRepository
    {
        Task<Chamada> GetByIdAsync(int id);
        Task<IEnumerable<Chamada>> GetAllAsync();
        Task AddAsync(Chamada chamada);
        Task UpdateAsync(Chamada chamada);
        Task DeleteAsync(int id);
    }
}
