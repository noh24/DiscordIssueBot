using Microsoft.AspNetCore.Mvc;
using FrontEndClient.Models;
using System.Collections.Generic;

namespace FrontEndClient.Controllers;

public class IssuessController : Controller
{
  public IActionResult Index()
  {
    List<Issue> issues = Issue.GetIssues();
    return View(issues);
  }

  public IActionResult Details(int id)
  {
    Issue issue = Issue.GetDetails(id);
    return View(issue);
  }
}
