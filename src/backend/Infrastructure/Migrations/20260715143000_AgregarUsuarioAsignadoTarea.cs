using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacionTareas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarUsuarioAsignadoTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO Usuarios (Id, Nombre, CreadoEnUtc, ActualizadoEnUtc)
                SELECT 1, 'Usuario legado', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
                WHERE NOT EXISTS (SELECT 1 FROM Usuarios WHERE Id = 1);
                """);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioAsignadoId",
                table: "Tareas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_UsuarioAsignadoId",
                table: "Tareas",
                column: "UsuarioAsignadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tareas_UsuarioAsignadoId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "UsuarioAsignadoId",
                table: "Tareas");
        }
    }
}