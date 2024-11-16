using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class Cleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fullname",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fullname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
