using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoOficinas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRelacionamentoChamadaAluno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAluno",
                table: "Chamadas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chamadas_IdAluno",
                table: "Chamadas",
                column: "IdAluno");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamadas_Alunos_IdAluno",
                table: "Chamadas",
                column: "IdAluno",
                principalTable: "Alunos",
                principalColumn: "IdAluno",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamadas_Alunos_IdAluno",
                table: "Chamadas");

            migrationBuilder.DropIndex(
                name: "IX_Chamadas_IdAluno",
                table: "Chamadas");

            migrationBuilder.DropColumn(
                name: "IdAluno",
                table: "Chamadas");
        }
    }
}
