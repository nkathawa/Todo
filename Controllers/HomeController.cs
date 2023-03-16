using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Dtos;
using Todo.Models;
using Todo.SyncDataServices.Http;

namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITodoDataClient _dataClient;
    private readonly AppDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, ITodoDataClient dataClient, AppDbContext dbContext)
    {
        _logger = logger;
        _dataClient = dataClient;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var todoItems = _dbContext.TodoItems.ToList();
        return View(todoItems);
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