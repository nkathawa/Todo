using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Todo.Dtos;
using Todo.Models;

namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoItemCreateDto todoItemCreateDto)
    {
        var httpClient = new HttpClient();
        var apiUrl = "http://localhost:5087/api/todo";

        var json = JsonSerializer.Serialize(todoItemCreateDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(apiUrl, content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("");
        }
        else
        {
            return View("Error");
        }
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}