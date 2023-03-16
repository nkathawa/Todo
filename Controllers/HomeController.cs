using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Todo.Dtos;
using Todo.Models;
using Todo.SyncDataServices.Http;

namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITodoDataClient _dataClient;

    public HomeController(ILogger<HomeController> logger, ITodoDataClient dataClient)
    {
        _logger = logger;
        _dataClient = dataClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoItemCreateDto todoItemCreateDto)
    {
        var response = await _dataClient.SendTodoItemFromWebToDb(todoItemCreateDto);
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