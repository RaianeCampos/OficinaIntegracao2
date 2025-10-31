namespace GestaoOficinas.Application.DTOs
{
    public class DocumentoViewModel
    {
        public int IdDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string StatusDocumento { get; set; }
        public DateTime Emissao { get; set; }
        public int IdOficina { get; set; }
        public int? IdEscola { get; set; }
        public int? IdAluno { get; set; }
        public string NomeOficina { get; set; }
    }

    public class CreateDocumentoDto
    {
        public string TipoDocumento { get; set; }
        public string StatusDocumento { get; set; } = "Gerado";
        public DateTime Emissao { get; set; } = DateTime.UtcNow;
        public int IdOficina { get; set; }
        public int? IdEscola { get; set; }
        public int? IdAluno { get; set; }
    }

    public class UpdateDocumentoDto
    {
        public string StatusDocumento { get; set; }
    }
}
