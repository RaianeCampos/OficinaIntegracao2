using GestaoOficinas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IPresencaRepository
    {
        Task<Presenca> GetByIdAsync(int idAluno, int idChamada);
        Task<IEnumerable<Presenca>> GetPresencasByChamadaAsync(int idChamada);
        Task<IEnumerable<Presenca>> GetPresencasByAlunoAsync(int idAluno);
        Task AddAsync(Presenca presenca);
        Task UpdateAsync(Presenca presenca);
        Task DeleteAsync(int idAluno, int idChamada);
    }
}
