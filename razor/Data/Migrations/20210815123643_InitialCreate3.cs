using Microsoft.EntityFrameworkCore.Migrations;

namespace razor.Data.Migrations
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "arpInInt",
                table: "NetworkAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "arpInInt",
                table: "NetworkAssignments");
        }
    }
}
