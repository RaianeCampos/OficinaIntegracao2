namespace GestaoOficinas.Web.Models
{
    public class RegistroChamadaViewModel
    {
        public int IdTurma { get; set; }
        public string NomeTurma { get; set; }

        public DateTime DataChamada { get; set; } = DateTime.UtcNow.Date;

        public List<AlunoPresencaItem> Alunos { get; set; } = new List<AlunoPresencaItem>();
    }

    public class AlunoPresencaItem
    {
        public int IdAluno { get; set; }
        public string NomeAluno { get; set; }
        public string RaAluno { get; set; }
        public bool Presente { get; set; } = true; 
    }
}