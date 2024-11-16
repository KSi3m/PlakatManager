using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postalcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_userid",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectionItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    size = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    cost = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    statusid = table.Column<int>(type: "int", nullable: false),
                    authorid = table.Column<int>(type: "int", nullable: false),
                    discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    refresh_rate = table.Column<int>(type: "int", nullable: true),
                    resolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paper_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_ElectionItems_Statuses_statusid",
                        column: x => x.statusid,
                        principalTable: "Statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectionItems_Users_authorid",
                        column: x => x.authorid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    updatedat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    electionitemid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_ElectionItems_electionitemid",
                        column: x => x.electionitemid,
                        principalTable: "ElectionItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectionItemTag",
                columns: table => new
                {
                    ElectionItemId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    date_of_publication = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionItemTag", x => new { x.TagId, x.ElectionItemId });
                    table.ForeignKey(
                        name: "FK_ElectionItemTag_ElectionItems_ElectionItemId",
                        column: x => x.ElectionItemId,
                        principalTable: "ElectionItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectionItemTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_userid",
                table: "Addresses",
                column: "userid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_electionitemid",
                table: "Comments",
                column: "electionitemid");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionItems_authorid",
                table: "ElectionItems",
                column: "authorid");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionItems_statusid",
                table: "ElectionItems",
                column: "statusid");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionItemTag_ElectionItemId",
                table: "ElectionItemTag",
                column: "ElectionItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ElectionItemTag");

            migrationBuilder.DropTable(
                name: "ElectionItems");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
