using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Domain.Entities
{
    public class Documento
    {
        [Key]
        public int IdDocumento { get; set; }
        public string TipoDocumento { get; set; } 
        public string StatusDocumento { get; set; } 
        public DateTime Emissao { get; set; }
        public int IdOficina { get; set; }
        public virtual Oficina Oficina { get; set; }
        public int? IdEscola { get; set; }
        public virtual Escola Escola { get; set; }
        public int? IdAluno { get; set; }
        public virtual Aluno Aluno { get; set; }
    }
}