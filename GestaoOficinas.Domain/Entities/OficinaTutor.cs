namespace GestaoOficinas.Domain.Entities
{
    public class OficinaTutor
    {
        public int IdOficina { get; set; }
        public int IdProfessor { get; set; }
        public virtual Oficina Oficina { get; set; }
        public virtual Professor Professor { get; set; }
    }
}