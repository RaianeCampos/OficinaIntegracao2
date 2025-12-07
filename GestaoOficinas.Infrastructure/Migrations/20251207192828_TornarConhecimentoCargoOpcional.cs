using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoOficinas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TornarConhecimentoCargoOpcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professores_Escolas_IdEscola",
                table: "Professores");

            migrationBuilder.AlterColumn<int>(
                name: "IdEscola",
                table: "Professores",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ConhecimentoProfessor",
                table: "Professores",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CargoProfessor",
                table: "Professores",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_Escolas_IdEscola",
                table: "Professores",
                column: "IdEscola",
                principalTable: "Escolas",
                principalColumn: "IdEscola");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professores_Escolas_IdEscola",
                table: "Professores");

            migrationBuilder.AlterColumn<int>(
                name: "IdEscola",
                table: "Professores",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConhecimentoProfessor",
                table: "Professores",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CargoProfessor",
                table: "Professores",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_Escolas_IdEscola",
                table: "Professores",
                column: "IdEscola",
                principalTable: "Escolas",
                principalColumn: "IdEscola",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
