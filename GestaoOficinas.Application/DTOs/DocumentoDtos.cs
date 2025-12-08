namespace GestaoOficinas.Application.DTOs
{
    public class DocumentoViewModel
    {
        public int IdDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string StatusDocumento { get; set; }
        public DateTime Emissao { get; set; }

        public string NomeOficina { get; set; }
        public string NomeProfessor { get; set; }
        public string? NomeAluno { get; set; }
        public string? NomeEscola { get; set; }
        public string? ResumoConteudo { get; set; }
    }

    public class CreateDocumentoDto
    {
        public string TipoDocumento { get; set; }
        public string StatusDocumento { get; set; } = "Gerado";
        public DateTime Emissao { get; set; } = DateTime.UtcNow;
        public int IdOficina { get; set; }
        public int? IdEscola { get; set; }
        public int? IdAluno { get; set; }
        public int IdProfessor { get; set; }
        public string NomeOficina { get; set; }
        public string TemaOficina { get; set; }
    }

    public class UpdateDocumentoDto
    {
        public string StatusDocumento { get; set; }
    }
}
