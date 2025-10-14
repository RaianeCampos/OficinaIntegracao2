namespace GestaoOficinas.Domain.Entities
{
    public class Presenca
    {
        public int IdAluno { get; set; }
        public int IdChamada { get; set; }
        public bool Presente { get; set; }
        public virtual Aluno Aluno { get; set; }
        public virtual Chamada Chamada { get; set; }
    }
}