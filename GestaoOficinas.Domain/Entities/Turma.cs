using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Domain.Entities
{
    public class Turma
    {
        [Key]
        public int IdTurma { get; set; }
        public string NomeTurma { get; set; }
        public string PeriodoTurma { get; set; } 
        public string SemestreTurma { get; set; } 
        public int IdOficina { get; set; }
        public virtual Oficina Oficina { get; set; }
        public virtual ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
        public virtual ICollection<Chamada> Chamadas { get; set; }
        public virtual ICollection<Inscricao> Inscricoes { get; set; }
    }
}