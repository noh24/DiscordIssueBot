using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndApi.Migrations
{
    public partial class AddSubjectToIssueModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Issues",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Issues");
        }
    }
}
