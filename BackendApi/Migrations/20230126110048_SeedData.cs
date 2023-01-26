using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndApi.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Subject" },
                values: new object[] { "Hello i've been trying to dev a bot in Discord.net and it's been doing good so far. Though issue that i had is that i'm trying to use the GetMessagesAsync from ITextChannel using an option. Unfortunately i have no idea how to initiate RequestOptions and i try to search the documentation and found nothing.", "Brian", "Discord.net cannot initiate RequestOptions" });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "Description", "Name", "Subject", "Token" },
                values: new object[] { 2, "How can I mention guild roles in C# with the Discord.net library?", "Yodel", "Discord.net how to mention roles", null });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "IssueId", "Description", "Name", "Subject", "Token" },
                values: new object[] { 3, "I'm trying to get a discord bot coded in discord.net running on a linux VPS, I'm running via mono but I keep getting this error", "Robert", "Discord.net not working on linux", null });

            migrationBuilder.UpdateData(
                table: "Solutions",
                keyColumn: "SolutionId",
                keyValue: 1,
                columns: new[] { "Description", "IssueId", "Name" },
                values: new object[] { "The As shown in the docs (https://docs.stillu.cc/guides/getting_started/installing.html), it's not possible to run Discord.Net with Mono. I suggest you to switch to .NET Core. When you'll now compile the code, it will output a .dll file (and regular .exe if 3.1+). Download .NET Core runtime on your distribution: https://learn.microsoft.com/en-us/dotnet/core/install/linux. Once it's installed you should be able to run the .dll file with the following command in the terminal: dotnet <path_to_dll>", 3, null });

            migrationBuilder.InsertData(
                table: "Solutions",
                columns: new[] { "SolutionId", "Description", "IssueId", "Name" },
                values: new object[] { 2, "It's also possible to use the raw mention of a role, that is in fact the content of the .Mention property in a role object. The format is the following: <@&ROLE_ID> It differs from mentioning an individual user by the & character, that specifies it's mentioning the role, not a user. You can get the role ID from the .ID property, or by right clicking the role in the roles list, if you want to hardcode it. Example command of mentioning a role by the name:", 2, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Solutions",
                keyColumn: "SolutionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Issues",
                keyColumn: "IssueId",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Subject" },
                values: new object[] { "Are the problem", "You", null });

            migrationBuilder.UpdateData(
                table: "Solutions",
                keyColumn: "SolutionId",
                keyValue: 1,
                columns: new[] { "Description", "IssueId", "Name" },
                values: new object[] { "The Solution", 1, "I am" });
        }
    }
}
