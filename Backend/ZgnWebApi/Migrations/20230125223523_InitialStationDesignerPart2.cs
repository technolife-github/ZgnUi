using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZgnWebApi.Migrations
{
    public partial class InitialStationDesignerPart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColPosition",
                table: "StationNodes",
                type: "int",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<int>(
                name: "RowPosition",
                table: "StationNodes",
                type: "int",
                nullable: false,
                defaultValue: -1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColPosition",
                table: "StationNodes");

            migrationBuilder.DropColumn(
                name: "RowPosition",
                table: "StationNodes");
        }
    }
}
