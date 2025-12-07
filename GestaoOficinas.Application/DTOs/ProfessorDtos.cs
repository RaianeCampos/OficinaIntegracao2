using System.ComponentModel.DataAnnotations;

namespace GestaoOficinas.Application.DTOs
{
    public class ProfessorViewModel
    {
        public int IdProfessor { get; set; }
        public string NomeProfessor { get; set; }
        public string EmailProfessor { get; set; }
        public bool Representante { get; set; }
        public int IdEscola { get; set; }
        public string? NomeEscola { get; set; }
        public string TelefoneProfessor { get; set; }
        public string? CargoProfessor { get; set; }
        public string? ConhecimentoProfessor { get; set; }
        public bool Administrador { get; set; }
    }

    public class CreateProfessorDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string NomeProfessor { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        public string EmailProfessor { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        public string TelefoneProfessor { get; set; } 

        public string? CargoProfessor { get; set; } 

        public string? ConhecimentoProfessor { get; set; } 

        [Required(ErrorMessage = "Selecione uma escola")]
        public int? IdEscola { get; set; }

        public bool Representante { get; set; }
        public bool Administrador { get; set; }
    }

    public class UpdateProfessorDto
    {
        public int IdProfessor { get; set; }
        public string NomeProfessor { get; set; }
        public string EmailProfessor { get; set; }
        public string TelefoneProfessor { get; set; }
        public string? CargoProfessor { get; set; }
        public string? ConhecimentoProfessor { get; set; }
        public int? IdEscola { get; set; }
        public bool Representante { get; set; }
        public bool Administrador { get; set; }
    }
}
