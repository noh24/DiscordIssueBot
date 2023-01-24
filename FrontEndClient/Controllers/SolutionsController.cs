using Microsoft.AspNetCore.Mvc;
using FrontEndClient.Models;

namespace FrontEndClient.Controllers;
public class SolutionsController : Controller
{
  [HttpGet("/Solutions")]
  public IActionResult Index()
  {
    List<Solution> solutions = Solution.GetSolutions();
    return View(solutions);
  }
  public IActionResult Details(int id)
  {
    Solution solution = Solution.GetDetails(id);
    return View(solution);
  }

}
