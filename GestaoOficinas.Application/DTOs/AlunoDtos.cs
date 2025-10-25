namespace GestaoOficinas.Application.DTOs
{
    public class AlunoViewModel
    {
        public int IdAluno { get; set; }
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public string RaAluno { get; set; }
        public int IdTurma { get; set; }
    }

    public class CreateAlunoDto
    {
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public string TelefoneAluno { get; set; }
        public string RaAluno { get; set; }
        public DateTime NascimentoAluno { get; set; }
        public int IdTurma { get; set; }
    }

    public class UpdateAlunoDto
    {
        public string NomeAluno { get; set; }
        public string EmailAluno { get; set; }
        public string TelefoneAluno { get; set; }
        public string RaAluno { get; set; }
        public DateTime NascimentoAluno { get; set; }
        public int IdTurma { get; set; }
    }
}
