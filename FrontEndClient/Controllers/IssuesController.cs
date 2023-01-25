using Microsoft.AspNetCore.Mvc;
using FrontEndClient.Models;
using System.Collections.Generic;

namespace FrontEndClient.Controllers;

public class IssuesController : Controller
{
  public ActionResult Index(string searchTerm)
  {
    if (!String.IsNullOrEmpty(searchTerm))
    {
      List<Issue> searchResult = Issue.SearchIssues(searchTerm);
      ViewBag.SearchTerm = searchTerm;
      return View(searchResult); 
    }
    else
    {
    List<Issue> issues = Issue.GetIssues();
    return View(issues);
    }
  }

  public IActionResult Details(int id, string searchTerm)
  {
    Issue issue = Issue.GetDetails(id);
    ViewBag.SearchTerm = searchTerm;    
    return View(issue);
  }
}
