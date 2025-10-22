using GestaoOficinas.Domain.Entities;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IInscricaoRepository
    {
        Task<Inscricao> GetByIdAsync(int idAluno, int idTurma);
        Task<IEnumerable<Inscricao>> GetAllAsync();
        Task AddAsync(Inscricao inscricao);
        Task UpdateAsync(Inscricao inscricao);
        Task DeleteAsync(int idAluno, int idTurma);
    }
}