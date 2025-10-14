using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Domain.Entities
{
    public class Professor
    {
        [Key]
        public int IdProfessor { get; set; }
        public string NomeProfessor { get; set; }
        public string EmailProfessor { get; set; }
        public string TelefoneProfessor { get; set; }
        public string ConhecimentoProfessor { get; set; } 
        public bool Representante { get; set; }
        public string CargoProfessor { get; set; }
        public int IdEscola { get; set; }
        public virtual Escola Escola { get; set; }
        public virtual ICollection<Oficina> OficinasResponsavel { get; set; }
        public virtual ICollection<OficinaTutor> OficinasComoTutor { get; set; }
    }
}