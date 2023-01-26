using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Models
{
  public class ApplicationContext : DbContext
  {
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Solution> Solutions { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Issue>()
        .HasData(
          new Issue { IssueId = 1, Subject = "Discord.net cannot initiate RequestOptions", Description = "Hello i've been trying to dev a bot in Discord.net and it's been doing good so far. Though issue that i had is that i'm trying to use the GetMessagesAsync from ITextChannel using an option. Unfortunately i have no idea how to initiate RequestOptions and i try to search the documentation and found nothing." },
          new Issue { IssueId = 2, Subject = "Discord.net how to mention roles", Description = "How can I mention guild roles in C# with the Discord.net library?"},
          new Issue { IssueId = 3, Subject = "Discord.net not working on linux", Description = "I'm trying to get a discord bot coded in discord.net running on a linux VPS, I'm running via mono but I keep getting this error"}
        );
      builder.Entity<Solution>()
        .HasData(
          new Solution { SolutionId = 1, Description = "The As shown in the docs (https://docs.stillu.cc/guides/getting_started/installing.html), it's not possible to run Discord.Net with Mono. I suggest you to switch to .NET Core. When you'll now compile the code, it will output a .dll file (and regular .exe if 3.1+). Download .NET Core runtime on your distribution: https://learn.microsoft.com/en-us/dotnet/core/install/linux. Once it's installed you should be able to run the .dll file with the following command in the terminal: dotnet <path_to_dll>", IssueId = 3 },
          new Solution { SolutionId = 2, Description = "It's also possible to use the raw mention of a role, that is in fact the content of the .Mention property in a role object. The format is the following: <@&ROLE_ID> It differs from mentioning an individual user by the & character, that specifies it's mentioning the role, not a user. You can get the role ID from the .ID property, or by right clicking the role in the roles list, if you want to hardcode it. Example command of mentioning a role by the name:", IssueId = 2}
        );
    }
  }
}