using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Joyeriaoro.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rol",
                table: "Usuarios",
                newName: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "Usuarios",
                newName: "Rol");
        }
    }
}
