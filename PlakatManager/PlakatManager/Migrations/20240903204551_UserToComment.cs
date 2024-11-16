using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class UserToComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "author",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "fullname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "authorid",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_authorid",
                table: "Comments",
                column: "authorid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_authorid",
                table: "Comments",
                column: "authorid",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_authorid",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_authorid",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "fullname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "authorid",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "author",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
