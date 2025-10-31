namespace GestaoOficinas.Application.DTOs
{
    public class InscricaoViewModel
    {
        public int IdAluno { get; set; }
        public int IdTurma { get; set; }
        public string StatusInscricao { get; set; }
        public string NomeAluno { get; set; }
        public string NomeTurma { get; set; }
    }

    public class CreateInscricaoDto
    {
        public int IdAluno { get; set; }
        public int IdTurma { get; set; }
        public string StatusInscricao { get; set; }
    }

    public class UpdateInscricaoDto
    {
        public string StatusInscricao { get; set; }
    }
}
