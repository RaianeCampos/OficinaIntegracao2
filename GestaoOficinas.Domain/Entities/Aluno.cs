using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Domain.Entities
{
    public class Aluno
    {
        [Key]
        public int IdAluno { get; set; }
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public string TelefoneAluno { get; set; }
        public string RaAluno { get; set; }
        public DateTime NascimentoAluno { get; set; }
        //public int IdTurma { get; set; }
        //public virtual Turma Turma { get; set; }
        public virtual ICollection<Turma> Turmas { get; set; } = new List<Turma>();
        public virtual ICollection<Inscricao> Inscricoes { get; set; }
        public virtual ICollection<Presenca> Presencas { get; set; }
    }
}