using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyCoordinateVariablesFromElectionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "ElectionItems");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "ElectionItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "ElectionItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "ElectionItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.Sql(@"
                 UPDATE ElectionItems
                 SET 
                      latitude = latitude_2,
                      longitude = longitude_2
                 WHERE latitude_2 IS NOT NULL AND longitude_2 IS NOT NULL;
             ");

           
        }
    }
}
