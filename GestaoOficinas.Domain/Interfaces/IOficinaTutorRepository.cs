using GestaoOficinas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoOficinas.Domain.Interfaces
{
    public interface IOficinaTutorRepository
    {
        Task<OficinaTutor> GetByIdAsync(int idOficina, int idProfessor);
        Task<IEnumerable<Professor>> GetTutoresByOficinaAsync(int idOficina);
        Task AddAsync(OficinaTutor oficinaTutor);
        Task DeleteAsync(int idOficina, int idProfessor);
    }
}
