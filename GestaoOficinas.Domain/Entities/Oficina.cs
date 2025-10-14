using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Domain.Entities
{
    public class Oficina
    {
        [Key]
        public int IdOficina { get; set; }
        public string NomeOficina { get; set; }
        public string TemaOficina { get; set; }
        public string DescricaoOficina { get; set; }
        public int CargaHorariaOficinia { get; set; }
        public DateTime DataOficina { get; set; }
        public string StatusOficina { get; set; } = "Em Andamento";
        public int IdProfessor { get; set; }
        public virtual Professor ProfessorResponsavel { get; set; }
        public virtual ICollection<Turma> Turmas { get; set; }
        public virtual ICollection<OficinaTutor> Tutores { get; set; }
        public virtual ICollection<Documento> Documentos { get; set; }
    }
}