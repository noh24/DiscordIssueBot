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
          new Issue { IssueId = 1, Name = "You", Description = "Are the problem" }
        );
      builder.Entity<Solution>()
        .HasData(
          new Solution { SolutionId = 1, Name = "I am", Description = "The Solution", IssueId = 1 }
        );
    }
  }
}