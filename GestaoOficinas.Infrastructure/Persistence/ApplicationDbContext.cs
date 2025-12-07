using GestaoOficinas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoOficinas.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Escola> Escolas { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Chamada> Chamadas { get; set; }
        public DbSet<Presenca> Presencas { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<OficinaTutor> OficinaTutores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Inscricao>()
                .HasKey(i => new { i.IdAluno, i.IdTurma });

            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.Aluno)
                .WithMany(a => a.Inscricoes)
                .HasForeignKey(i => i.IdAluno);

            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.Turma)
                .WithMany(t => t.Inscricoes)
                .HasForeignKey(i => i.IdTurma);

            modelBuilder.Entity<Presenca>()
                .HasKey(p => new { p.IdAluno, p.IdChamada });

            modelBuilder.Entity<Presenca>()
                .HasOne(p => p.Aluno)
                .WithMany(a => a.Presencas)
                .HasForeignKey(p => p.IdAluno);

            modelBuilder.Entity<Presenca>()
                .HasOne(p => p.Chamada)
                .WithMany(c => c.Presencas)
                .HasForeignKey(p => p.IdChamada);

            modelBuilder.Entity<OficinaTutor>()
                .HasKey(ot => new { ot.IdOficina, ot.IdProfessor });

            modelBuilder.Entity<OficinaTutor>()
                .HasOne(ot => ot.Oficina)
                .WithMany(o => o.Tutores)
                .HasForeignKey(ot => ot.IdOficina);

            modelBuilder.Entity<OficinaTutor>()
                .HasOne(ot => ot.Professor)
                .WithMany(p => p.OficinasComoTutor)
                .HasForeignKey(ot => ot.IdProfessor);

            modelBuilder.Entity<Escola>()
                .HasMany(e => e.Professores)
                .WithOne(p => p.Escola)
                .HasForeignKey(p => p.IdEscola);

            modelBuilder.Entity<Aluno>()
                .HasMany(a => a.Turmas)
                .WithMany(t => t.Alunos)
                .UsingEntity(j => j.ToTable("AlunoTurmas"));

            modelBuilder.Entity<Oficina>()
                .HasMany(o => o.Turmas)
                .WithOne(t => t.Oficina)
                .HasForeignKey(t => t.IdOficina);

            modelBuilder.Entity<Professor>()
                .HasMany(p => p.OficinasResponsavel)
                .WithOne(o => o.ProfessorResponsavel)
                .HasForeignKey(o => o.IdProfessor)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Turma>()
                .HasMany(t => t.Chamadas)
                .WithOne(c => c.Turma)
                .HasForeignKey(c => c.IdTurma);

            modelBuilder.Entity<Oficina>()
                .HasMany(o => o.Documentos)
                .WithOne(d => d.Oficina)
                .HasForeignKey(d => d.IdOficina);

            modelBuilder.Entity<Documento>()
                .HasOne(d => d.Escola)
                .WithMany() 
                .HasForeignKey(d => d.IdEscola)
                .IsRequired(false);

            modelBuilder.Entity<Documento>()
                .HasOne(d => d.Aluno)
                .WithMany()
                .HasForeignKey(d => d.IdAluno)
                .IsRequired(false); 
        }
    }
}