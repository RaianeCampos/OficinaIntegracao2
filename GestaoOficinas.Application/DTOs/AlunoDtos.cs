namespace GestaoOficinas.Application.DTOs
{
    public class AlunoViewModel
    {
        public int IdAluno { get; set; }
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public string RaAluno { get; set; }
        public List<string> NomesTurmas { get; set; } = new List<string>();
        public List<int> TurmaIds { get; set; } = new List<int>();
        public string? NomeTurma { get; set; }
        public string TelefoneAluno { get; set; }
        public DateTime NascimentoAluno { get; set; }
    }

    public class CreateAlunoDto
    {
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public string TelefoneAluno { get; set; }
        public string RaAluno { get; set; }
        public DateTime NascimentoAluno { get; set; }
        public List<int> TurmaIds { get; set; } = new List<int>();
    }

    public class UpdateAlunoDto
    {
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public string TelefoneAluno { get; set; }
        public string RaAluno { get; set; }
        public DateTime NascimentoAluno { get; set; }
        public List<int> TurmaIds { get; set; } = new List<int>();
    }
}
