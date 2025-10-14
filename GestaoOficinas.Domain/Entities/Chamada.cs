using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Domain.Entities
{
    public class Chamada
    {
        [Key]
        public int IdChamada { get; set; }
        public DateTime DataChamada { get; set; }
        public int IdTurma { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual ICollection<Presenca> Presencas { get; set; }
    }
}