using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZgnWebApi.Migrations
{
    public partial class InitialStationDesignerPart1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColumnLen",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowLen",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnLen",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "RowLen",
                table: "Stations");
        }
    }
}
