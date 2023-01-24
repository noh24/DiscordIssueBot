using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrontEndClient.Models
{
  public class Solution
  {
    public int SolutionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int IssueId { get; set; }
    public Issue Issue { get; set; }

    public static List<Solution> GetSolutions()
    {
      var apiCallTask = ApiHelper.GetAll("solutions");
      var result = apiCallTask.Result;

      JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(result);
      List<Solution> solutionList = JsonConvert.DeserializeObject<List<Solution>>(jsonResponse.ToString());

      return solutionList;
    }

    public static Solution GetDetails(int id)
    {
      var apiCallTask = ApiHelper.Get("solutions", id);
      var result = apiCallTask.Result;

      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      Solution solution = JsonConvert.DeserializeObject<Solution>(jsonResponse.ToString());

      return solution;
    }
  }
}
