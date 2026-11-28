using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OptimizarConsultasCitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DiaSemana",
                table: "HorariosMedicos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosMedicos_Medico_Dia_Estado",
                table: "HorariosMedicos",
                columns: new[] { "IdMedico", "DiaSemana", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_Fecha_Consultorio",
                table: "Citas",
                columns: new[] { "FechaCita", "IdConsultorio" });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_Fecha_Medico",
                table: "Citas",
                columns: new[] { "FechaCita", "IdMedico" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HorariosMedicos_Medico_Dia_Estado",
                table: "HorariosMedicos");

            migrationBuilder.DropIndex(
                name: "IX_Citas_Fecha_Consultorio",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_Fecha_Medico",
                table: "Citas");

            migrationBuilder.AlterColumn<string>(
                name: "DiaSemana",
                table: "HorariosMedicos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
