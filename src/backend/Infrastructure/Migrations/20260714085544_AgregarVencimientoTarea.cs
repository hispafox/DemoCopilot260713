using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacionTareas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarVencimientoTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VenceEnUtc",
                table: "Tareas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_VenceEnUtc",
                table: "Tareas",
                column: "VenceEnUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tareas_VenceEnUtc",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "VenceEnUtc",
                table: "Tareas");
        }
    }
}
