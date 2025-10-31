using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestaoOficinas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Escolas",
                columns: table => new
                {
                    IdEscola = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeEscola = table.Column<string>(type: "text", nullable: false),
                    CnpjEscola = table.Column<string>(type: "text", nullable: false),
                    CepEscola = table.Column<string>(type: "text", nullable: false),
                    RuaEscola = table.Column<string>(type: "text", nullable: false),
                    ComplementoEscola = table.Column<string>(type: "text", nullable: false),
                    TelefoneEscola = table.Column<string>(type: "text", nullable: false),
                    EmailEscola = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolas", x => x.IdEscola);
                });

            migrationBuilder.CreateTable(
                name: "Professores",
                columns: table => new
                {
                    IdProfessor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeProfessor = table.Column<string>(type: "text", nullable: false),
                    EmailProfessor = table.Column<string>(type: "text", nullable: false),
                    TelefoneProfessor = table.Column<string>(type: "text", nullable: false),
                    ConhecimentoProfessor = table.Column<string>(type: "text", nullable: false),
                    Representante = table.Column<bool>(type: "boolean", nullable: false),
                    CargoProfessor = table.Column<string>(type: "text", nullable: false),
                    IdEscola = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professores", x => x.IdProfessor);
                    table.ForeignKey(
                        name: "FK_Professores_Escolas_IdEscola",
                        column: x => x.IdEscola,
                        principalTable: "Escolas",
                        principalColumn: "IdEscola",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oficinas",
                columns: table => new
                {
                    IdOficina = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeOficina = table.Column<string>(type: "text", nullable: false),
                    TemaOficina = table.Column<string>(type: "text", nullable: false),
                    DescricaoOficina = table.Column<string>(type: "text", nullable: false),
                    CargaHorariaOficinia = table.Column<int>(type: "integer", nullable: false),
                    DataOficina = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusOficina = table.Column<string>(type: "text", nullable: false),
                    IdProfessor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oficinas", x => x.IdOficina);
                    table.ForeignKey(
                        name: "FK_Oficinas_Professores_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Professores",
                        principalColumn: "IdProfessor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OficinaTutores",
                columns: table => new
                {
                    IdOficina = table.Column<int>(type: "integer", nullable: false),
                    IdProfessor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OficinaTutores", x => new { x.IdOficina, x.IdProfessor });
                    table.ForeignKey(
                        name: "FK_OficinaTutores_Oficinas_IdOficina",
                        column: x => x.IdOficina,
                        principalTable: "Oficinas",
                        principalColumn: "IdOficina",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OficinaTutores_Professores_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Professores",
                        principalColumn: "IdProfessor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turmas",
                columns: table => new
                {
                    IdTurma = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeTurma = table.Column<string>(type: "text", nullable: false),
                    PeriodoTurma = table.Column<string>(type: "text", nullable: false),
                    SemestreTurma = table.Column<string>(type: "text", nullable: false),
                    IdOficina = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turmas", x => x.IdTurma);
                    table.ForeignKey(
                        name: "FK_Turmas_Oficinas_IdOficina",
                        column: x => x.IdOficina,
                        principalTable: "Oficinas",
                        principalColumn: "IdOficina",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    IdAluno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeAluno = table.Column<string>(type: "text", nullable: false),
                    EmailAluno = table.Column<string>(type: "text", nullable: false),
                    TelefoneAluno = table.Column<string>(type: "text", nullable: false),
                    RaAluno = table.Column<string>(type: "text", nullable: false),
                    NascimentoAluno = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdTurma = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.IdAluno);
                    table.ForeignKey(
                        name: "FK_Alunos_Turmas_IdTurma",
                        column: x => x.IdTurma,
                        principalTable: "Turmas",
                        principalColumn: "IdTurma",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chamadas",
                columns: table => new
                {
                    IdChamada = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataChamada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdTurma = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chamadas", x => x.IdChamada);
                    table.ForeignKey(
                        name: "FK_Chamadas_Turmas_IdTurma",
                        column: x => x.IdTurma,
                        principalTable: "Turmas",
                        principalColumn: "IdTurma",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    IdDocumento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoDocumento = table.Column<string>(type: "text", nullable: false),
                    StatusDocumento = table.Column<string>(type: "text", nullable: false),
                    Emissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdOficina = table.Column<int>(type: "integer", nullable: false),
                    IdEscola = table.Column<int>(type: "integer", nullable: true),
                    IdAluno = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.IdDocumento);
                    table.ForeignKey(
                        name: "FK_Documentos_Alunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "Alunos",
                        principalColumn: "IdAluno");
                    table.ForeignKey(
                        name: "FK_Documentos_Escolas_IdEscola",
                        column: x => x.IdEscola,
                        principalTable: "Escolas",
                        principalColumn: "IdEscola");
                    table.ForeignKey(
                        name: "FK_Documentos_Oficinas_IdOficina",
                        column: x => x.IdOficina,
                        principalTable: "Oficinas",
                        principalColumn: "IdOficina",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscricoes",
                columns: table => new
                {
                    IdAluno = table.Column<int>(type: "integer", nullable: false),
                    IdTurma = table.Column<int>(type: "integer", nullable: false),
                    StatusInscricao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscricoes", x => new { x.IdAluno, x.IdTurma });
                    table.ForeignKey(
                        name: "FK_Inscricoes_Alunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "Alunos",
                        principalColumn: "IdAluno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Turmas_IdTurma",
                        column: x => x.IdTurma,
                        principalTable: "Turmas",
                        principalColumn: "IdTurma",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Presencas",
                columns: table => new
                {
                    IdAluno = table.Column<int>(type: "integer", nullable: false),
                    IdChamada = table.Column<int>(type: "integer", nullable: false),
                    Presente = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presencas", x => new { x.IdAluno, x.IdChamada });
                    table.ForeignKey(
                        name: "FK_Presencas_Alunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "Alunos",
                        principalColumn: "IdAluno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Presencas_Chamadas_IdChamada",
                        column: x => x.IdChamada,
                        principalTable: "Chamadas",
                        principalColumn: "IdChamada",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_IdTurma",
                table: "Alunos",
                column: "IdTurma");

            migrationBuilder.CreateIndex(
                name: "IX_Chamadas_IdTurma",
                table: "Chamadas",
                column: "IdTurma");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdAluno",
                table: "Documentos",
                column: "IdAluno");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdEscola",
                table: "Documentos",
                column: "IdEscola");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_IdOficina",
                table: "Documentos",
                column: "IdOficina");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_IdTurma",
                table: "Inscricoes",
                column: "IdTurma");

            migrationBuilder.CreateIndex(
                name: "IX_Oficinas_IdProfessor",
                table: "Oficinas",
                column: "IdProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_OficinaTutores_IdProfessor",
                table: "OficinaTutores",
                column: "IdProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_IdChamada",
                table: "Presencas",
                column: "IdChamada");

            migrationBuilder.CreateIndex(
                name: "IX_Professores_IdEscola",
                table: "Professores",
                column: "IdEscola");

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_IdOficina",
                table: "Turmas",
                column: "IdOficina");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Inscricoes");

            migrationBuilder.DropTable(
                name: "OficinaTutores");

            migrationBuilder.DropTable(
                name: "Presencas");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "Chamadas");

            migrationBuilder.DropTable(
                name: "Turmas");

            migrationBuilder.DropTable(
                name: "Oficinas");

            migrationBuilder.DropTable(
                name: "Professores");

            migrationBuilder.DropTable(
                name: "Escolas");
        }
    }
}
