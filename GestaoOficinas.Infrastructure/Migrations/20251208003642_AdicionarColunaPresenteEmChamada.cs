using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoOficinas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarColunaPresenteEmChamada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Presente",
                table: "Chamadas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Presente",
                table: "Chamadas");
        }
    }
}
