namespace BackEndApi.Models
{
  public class Issue
  {
    public int IssueId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    List<Solution> Solutions {get; set; }
  }
}