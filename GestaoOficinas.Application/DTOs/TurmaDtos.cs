namespace GestaoOficinas.Application.DTOs
{
    public class TurmaViewModel
    {
        public int IdTurma { get; set; }
        public string NomeTurma { get; set; }
        public int IdOficina { get; set; }
        public string NomeOficina { get; set; }
        public string PeriodoTurma { get; set; }
        public string SemestreTurma { get; set; }
    }

    public class CreateTurmaDto
    {
        public string NomeTurma { get; set; }
        public string PeriodoTurma { get; set; }
        public string SemestreTurma { get; set; }
        public int IdOficina { get; set; }
    }

    public class UpdateTurmaDto
    {
        public string NomeTurma { get; set; }
        public string PeriodoTurma { get; set; }
        public string SemestreTurma { get; set; }
    }
}
