namespace GestaoOficinas.Application.DTOs
{
    public class ChamadaViewModel
    {
        public int IdChamada { get; set; }
        public DateTime DataChamada { get; set; }
        public int IdTurma { get; set; }
        public string? NomeTurma { get; set; }
        public int IdAluno { get; set; }
        public string? NomeAluno { get; set; } 
        public bool Presente { get; set; }     
    }

    public class CreateChamadaDto
    {
        public DateTime DataChamada { get; set; }
        public int IdTurma { get; set; }
        public int IdAluno { get; set; } 
        public bool Presente { get; set; } 
    }
}