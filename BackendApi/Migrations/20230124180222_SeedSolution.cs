using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndApi.Migrations
{
    public partial class SeedSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Solutions",
                columns: new[] { "SolutionId", "Description", "IssueId", "Name" },
                values: new object[] { 1, "The Solution", 1, "I am" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Solutions",
                keyColumn: "SolutionId",
                keyValue: 1);
        }
    }
}
