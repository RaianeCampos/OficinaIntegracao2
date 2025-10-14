using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Domain.Entities
{
    public class Escola
    {
        [Key]
        public int IdEscola { get; set; }
        public string NomeEscola { get; set; }
        public string CnpjEscola { get; set; }
        public string CepEscola { get; set; }
        public string RuaEscola { get; set; }
        public string ComplementoEscola { get; set; }
        public string TelefoneEscola { get; set; }
        public string EmailEscola { get; set; }
        public virtual ICollection<Professor> Professores { get; set; }
    }
}