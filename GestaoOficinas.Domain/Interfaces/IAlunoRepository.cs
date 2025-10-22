using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IAlunoRepository
    {
        Task<Aluno> GetByIdAsync(int id);
        Task<IEnumerable<Aluno>> GetAllAsync();
        Task AddAsync(Aluno aluno);
        Task UpdateAsync(Aluno aluno);
        Task DeleteAsync(int id);
    }
}
