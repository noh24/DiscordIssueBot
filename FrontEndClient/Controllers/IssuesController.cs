using Microsoft.AspNetCore.Mvc;
using FrontEndClient.Models;
using System.Collections.Generic;

namespace FrontEndClient.Controllers;

public class IssuesController : Controller
{
  [HttpGet("/issues")]
  public IActionResult Index()
  {
    List<Issue> issues = Issue.GetIssues();
    return View(issues);
  }

  [HttpGet]
  public IActionResult Search()
  {
    return View();
  }

  [HttpPost, ActionName("Search")]
  public IActionResult Search(string search)
  {
    return RedirectToAction("SearchResults", "Issues", new {searchTerm = search}); 
  }

  [HttpGet]
  public IActionResult SearchResults(string searchTerm)
  {
    List<Issue> searchResult = Issue.SearchIssues(searchTerm);
    ViewBag.SearchTerm = searchTerm;
    return View(searchResult); 
  }

  public IActionResult Details(int id)
  {
    Issue issue = Issue.GetDetails(id);
    return View(issue);
  }
}
