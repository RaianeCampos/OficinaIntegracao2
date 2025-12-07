using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoOficinas.Domain.Entities
{
    public class Chamada
    {
        [Key]
        public int IdChamada { get; set; }
        public DateTime DataChamada { get; set; }
        public int IdTurma { get; set; }

        [ForeignKey("IdTurma")]
        public virtual Turma Turma { get; set; }
        public virtual ICollection<Presenca> Presencas { get; set; }
        public int IdAluno { get; set; } 

        [ForeignKey("IdAluno")]
        public virtual Aluno Aluno { get; set; }
    }
}