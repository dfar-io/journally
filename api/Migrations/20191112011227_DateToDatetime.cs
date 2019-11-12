using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class DateToDatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Entries",
                newName: "DateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Entries",
                newName: "Date");
        }
    }
}
