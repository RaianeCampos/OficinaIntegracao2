using GestaoOficinas.Domain.Entities;
using GestaoOficinas.Domain.Interfaces;
using GestaoOficinas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Infrastructure.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly ApplicationDbContext _context;
        public AlunoRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddAsync(Aluno aluno)
        {
            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var aluno = await GetByIdAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Aluno>> GetAllAsync()
        {
            return await _context.Alunos
                .Include(a => a.Turmas)
                .AsNoTracking()         
                .ToListAsync();
        }

        public async Task<IEnumerable<Aluno>> GetAllWithTurmaAsync()
        {
            return await _context.Alunos
                .Include(a => a.Turmas)
                .AsNoTracking()
                .ToListAsync();
        }

        
        public async Task<Aluno> GetByIdAsync(int id)
        {
            return await _context.Alunos
                .Include(a => a.Turmas)
                .FirstOrDefaultAsync(a => a.IdAluno == id);
        }

        public async Task UpdateAsync(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}