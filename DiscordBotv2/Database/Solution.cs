namespace DiscordBot.Models
{
  public class Solution
  {
    public int IssueId { get; set; }
    public Issue Issue { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int SolutionId { get; set; }
  }
}