using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface ITurmaRepository
    {
        Task<Turma> GetByIdAsync(int id);
        Task<IEnumerable<Turma>> GetAllAsync();
        Task AddAsync(Turma turma);
        Task UpdateAsync(Turma turma);
        Task DeleteAsync(int id);
    }
}