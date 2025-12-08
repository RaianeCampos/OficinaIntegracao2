using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoOficinas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteDocumentoProfessor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaminhoArquivo",
                table: "Documentos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdProfessor",
                table: "Documentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ResumoConteudo",
                table: "Documentos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdProfessor",
                table: "Documentos",
                column: "IdProfessor");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Professores_IdProfessor",
                table: "Documentos",
                column: "IdProfessor",
                principalTable: "Professores",
                principalColumn: "IdProfessor",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Professores_IdProfessor",
                table: "Documentos");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_IdProfessor",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "CaminhoArquivo",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "IdProfessor",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "ResumoConteudo",
                table: "Documentos");
        }
    }
}
