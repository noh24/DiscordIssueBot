using Microsoft.EntityFrameworkCore;

namespace TestDiscord.Repository
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
          new Issue { IssueId = 1, Name = "You", Description = "Are the problem" }
        );
    }
  }
}