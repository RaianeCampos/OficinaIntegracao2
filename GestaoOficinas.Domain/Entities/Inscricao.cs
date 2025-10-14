namespace GestaoOficinas.Domain.Entities
{
    public class Inscricao
    {
        public int IdAluno { get; set; }
        public int IdTurma { get; set; }
        public string StatusInscricao { get; set; }
        public virtual Aluno Aluno { get; set; }
        public virtual Turma Turma { get; set; }
    }
}