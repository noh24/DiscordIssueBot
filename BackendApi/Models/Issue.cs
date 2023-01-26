namespace BackEndApi.Models
{
  public class Issue
  {
    public int IssueId { get; set; }
    public string Token { get; set; }
    public string Subject { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Solution> Solutions {get; set; }
  }
}