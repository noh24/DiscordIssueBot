﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FrontEndClient.Models;

namespace FrontEndClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    
    public IActionResult Index()
    {
            List<Issue> issues = Issue.GetIssues();
            return View(issues);
    }


    public IActionResult Documentation()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
