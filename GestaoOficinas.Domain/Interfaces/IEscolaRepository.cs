using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IEscolaRepository
    {
        Task<Escola> GetByIdAsync(int id);
        Task<IEnumerable<Escola>> GetAllAsync();
        Task AddAsync(Escola escola);
        Task UpdateAsync(Escola escola);
        Task DeleteAsync(int id);
        Task<bool> CnpjExistsAsync(string cnpj);
    }
}