using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoOficinas.Domain.Entities
{
    public class Documento
    {
        [Key]
        public int IdDocumento { get; set; }

        public string TipoDocumento { get; set; } 
        public string StatusDocumento { get; set; } 
        public DateTime Emissao { get; set; } = DateTime.UtcNow;

        public string? CaminhoArquivo { get; set; }
        public string? ResumoConteudo { get; set; } 

        public int IdOficina { get; set; }
        [ForeignKey("IdOficina")]
        public virtual Oficina Oficina { get; set; }

        public int? IdEscola { get; set; }
        [ForeignKey("IdEscola")]
        public virtual Escola Escola { get; set; }

        public int? IdAluno { get; set; }
        [ForeignKey("IdAluno")]
        public virtual Aluno Aluno { get; set; }

        public int IdProfessor { get; set; }
        [ForeignKey("IdProfessor")]
        public virtual Professor Professor { get; set; }
    }
}