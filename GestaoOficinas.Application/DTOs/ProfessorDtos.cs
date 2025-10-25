namespace GestaoOficinas.Application.DTOs
{
    public class ProfessorViewModel
    {
        public int IdProfessor { get; set; }
        public string NomeProfessor { get; set; }
        public string EmailProfessor { get; set; }
        public bool Representante { get; set; }
        public int IdEscola { get; set; }
    }

    public class CreateProfessorDto
    {
        public string NomeProfessor { get; set; }
        public string EmailProfessor { get; set; }
        public string TelefoneProfessor { get; set; }
        public string ConhecimentoProfessor { get; set; }
        public bool Representante { get; set; }
        public string CargoProfessor { get; set; }
        public int IdEscola { get; set; }
    }

    public class UpdateProfessorDto
    {
        public string NomeProfessor { get; set; }
        public string EmailProfessor { get; set; }
        public string TelefoneProfessor { get; set; }
        public string ConhecimentoProfessor { get; set; }
        public bool Representante { get; set; }
        public string CargoProfessor { get; set; }
    }
}
