using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectionItemTag_ElectionItems_ElectionItemId",
                table: "ElectionItemTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionItemTag_Tags_TagId",
                table: "ElectionItemTag");

            migrationBuilder.RenameColumn(
                name: "ElectionItemId",
                table: "ElectionItemTag",
                newName: "electionitemid");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "ElectionItemTag",
                newName: "tagid");

            migrationBuilder.RenameIndex(
                name: "IX_ElectionItemTag_ElectionItemId",
                table: "ElectionItemTag",
                newName: "IX_ElectionItemTag_electionitemid");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalizedname = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    concurrencystamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalizedusername = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalizedemail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    emailconfirmed = table.Column<bool>(type: "bit", nullable: false),
                    passwordhash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    securitystamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concurrencystamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phonenumberconfirmed = table.Column<bool>(type: "bit", nullable: false),
                    twofactorenabled = table.Column<bool>(type: "bit", nullable: false),
                    lockoutend = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    lockoutenabled = table.Column<bool>(type: "bit", nullable: false),
                    accessfailedcount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    claimtype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    claimvalue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_roleid",
                        column: x => x.roleid,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    claimtype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    claimvalue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    loginprovider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    providerkey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    providerdisplayname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userid = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.loginprovider, x.providerkey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    roleid = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_roleid",
                        column: x => x.roleid,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    loginprovider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.userid, x.loginprovider, x.name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_roleid",
                table: "AspNetRoleClaims",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalizedname",
                unique: true,
                filter: "[normalizedname] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_userid",
                table: "AspNetUserClaims",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_userid",
                table: "AspNetUserLogins",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_roleid",
                table: "AspNetUserRoles",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalizedemail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalizedusername",
                unique: true,
                filter: "[normalizedusername] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionItemTag_ElectionItems_electionitemid",
                table: "ElectionItemTag",
                column: "electionitemid",
                principalTable: "ElectionItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionItemTag_Tags_tagid",
                table: "ElectionItemTag",
                column: "tagid",
                principalTable: "Tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElectionItemTag_ElectionItems_electionitemid",
                table: "ElectionItemTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ElectionItemTag_Tags_tagid",
                table: "ElectionItemTag");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "electionitemid",
                table: "ElectionItemTag",
                newName: "ElectionItemId");

            migrationBuilder.RenameColumn(
                name: "tagid",
                table: "ElectionItemTag",
                newName: "TagId");

            migrationBuilder.RenameIndex(
                name: "IX_ElectionItemTag_electionitemid",
                table: "ElectionItemTag",
                newName: "IX_ElectionItemTag_ElectionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionItemTag_ElectionItems_ElectionItemId",
                table: "ElectionItemTag",
                column: "ElectionItemId",
                principalTable: "ElectionItems",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElectionItemTag_Tags_TagId",
                table: "ElectionItemTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
