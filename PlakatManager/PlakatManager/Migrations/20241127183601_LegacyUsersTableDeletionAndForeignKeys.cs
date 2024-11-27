using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class LegacyUsersTableDeletionAndForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_userid",
                table: "Addresses",
                column: "userid",
                principalTable: "AspNetUsers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_authorid",
                table: "Comments",
                column: "authorid",
                principalTable: "AspNetUsers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionItems_AspNetUsers_authorid",
                table: "ElectionItems",
                column: "authorid",
                principalTable: "AspNetUsers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_userid",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_authorid",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionItems_AspNetUsers_authorid",
                table: "ElectionItems");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });
        }
    }
}
