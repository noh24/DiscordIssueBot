using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrontEndClient.Models
{
  public class Issue
  {
    public int IssueId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Solution> Solutions {get; set; }

    public static List<Issue> GetIssues()
    {
      string route = "Issues";
      var apiCallTask = ApiHelper.GetAll(route);
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Issue> issueList = JsonConvert.DeserializeObject<List<Issue>>(jsonResponse.ToString());

      return issueList;
    }

    public static List<Issue> SearchIssues(string search)
    {
      string route = "Issues";
      var apiCallTask = ApiHelper.Search(route, search);
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Issue> issueList = JsonConvert.DeserializeObject<List<Issue>>(jsonResponse.ToString());

      return issueList;      
    }

    public static Issue GetDetails(int id)
    {
      var apiCallTask = ApiHelper.Get("issues", id);
      var result = apiCallTask.Result;

      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      Issue issue = JsonConvert.DeserializeObject<Issue>(jsonResponse.ToString());
      return issue;
    }
  }
}
