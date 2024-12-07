using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class NamingAndCityFieldInLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "longitude_2",
                table: "ElectionItems",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "latitude_2",
                table: "ElectionItems",
                newName: "latitude");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "ElectionItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "ElectionItems");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "ElectionItems",
                newName: "longitude_2");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "ElectionItems",
                newName: "latitude_2");
        }
    }
}
