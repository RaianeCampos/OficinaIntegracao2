using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class OficinaTutorRepository : IOficinaTutorRepository
    {
        private readonly ApplicationDbContext _context;

        public OficinaTutorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OficinaTutor oficinaTutor)
        {
            var existe = await GetByIdAsync(oficinaTutor.IdOficina, oficinaTutor.IdProfessor);
            if (existe == null)
            {
                await _context.OficinaTutores.AddAsync(oficinaTutor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int idOficina, int idProfessor)
        {
            var oficinaTutor = await GetByIdAsync(idOficina, idProfessor);
            if (oficinaTutor != null)
            {
                _context.OficinaTutores.Remove(oficinaTutor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OficinaTutor> GetByIdAsync(int idOficina, int idProfessor)
        {
            return await _context.OficinaTutores
                .FindAsync(idOficina, idProfessor);
        }

        public async Task<IEnumerable<Professor>> GetTutoresByOficinaAsync(int idOficina)
        {
            return await _context.OficinaTutores
                .Where(ot => ot.IdOficina == idOficina)
                .Select(ot => ot.Professor) 
                .ToListAsync();
        }
    }
}
