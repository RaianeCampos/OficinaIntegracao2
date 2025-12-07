using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoOficinas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteAlunoTurmasNN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Turmas_IdTurma",
                table: "Alunos");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_IdTurma",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "IdTurma",
                table: "Alunos");

            migrationBuilder.CreateTable(
                name: "AlunoTurmas",
                columns: table => new
                {
                    AlunosIdAluno = table.Column<int>(type: "integer", nullable: false),
                    TurmasIdTurma = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlunoTurmas", x => new { x.AlunosIdAluno, x.TurmasIdTurma });
                    table.ForeignKey(
                        name: "FK_AlunoTurmas_Alunos_AlunosIdAluno",
                        column: x => x.AlunosIdAluno,
                        principalTable: "Alunos",
                        principalColumn: "IdAluno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlunoTurmas_Turmas_TurmasIdTurma",
                        column: x => x.TurmasIdTurma,
                        principalTable: "Turmas",
                        principalColumn: "IdTurma",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlunoTurmas_TurmasIdTurma",
                table: "AlunoTurmas",
                column: "TurmasIdTurma");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlunoTurmas");

            migrationBuilder.AddColumn<int>(
                name: "IdTurma",
                table: "Alunos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_IdTurma",
                table: "Alunos",
                column: "IdTurma");

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Turmas_IdTurma",
                table: "Alunos",
                column: "IdTurma",
                principalTable: "Turmas",
                principalColumn: "IdTurma",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
