using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZgnWebApi.Migrations
{
    public partial class TransactionDescriptionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Transactions",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transactions",
                newName: "ProductName");

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
