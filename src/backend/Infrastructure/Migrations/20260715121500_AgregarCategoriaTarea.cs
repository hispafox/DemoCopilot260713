using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacionTareas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCategoriaTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Tareas",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "General");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_Categoria",
                table: "Tareas",
                column: "Categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tareas_Categoria",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Tareas");
        }
    }
}