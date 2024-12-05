using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionMaterialManager.Migrations
{
    /// <inheritdoc />
    public partial class LocationForElectionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "ElectionItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "district",
                table: "ElectionItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
               name: "street",
               table: "ElectionItems",
               type: "nvarchar(max)",
               nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "latitude_2",
                table: "ElectionItems",
                type: "float(10)",
                precision: 10,
                scale: 5,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "longitude_2",
                table: "ElectionItems",
                type: "float(10)",
                precision: 10,
                scale: 5,
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ElectionItems
                     SET 
                        latitude_2 = CAST(latitude AS FLOAT),
                        longitude_2 = CAST(longitude AS FLOAT);
                ");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "ElectionItems");

            migrationBuilder.DropColumn(
                name: "district",
                table: "ElectionItems");

            migrationBuilder.DropColumn(
                name: "latitude_2",
                table: "ElectionItems");

            migrationBuilder.DropColumn(
                name: "longitude_2",
                table: "ElectionItems");

            migrationBuilder.DropColumn(
                name: "street",
                table: "ElectionItems");
        }
    }
}
