using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IProfessorRepository
    {
        Task<Professor> GetByIdAsync(int id);
        Task<IEnumerable<Professor>> GetAllAsync();
        Task AddAsync(Professor professor);
        Task UpdateAsync(Professor professor);
        Task DeleteAsync(int id);
    }
}
