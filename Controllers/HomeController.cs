using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger,
        ITodoDataClient dataClient,
        AppDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _dataClient = dataClient;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        Console.WriteLine("hello0");
        if (ModelState.IsValid)
        {
            Console.WriteLine("hello1");
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                Console.WriteLine("hello2");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await _userManager.AddClaimsAsync(user, claims);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                Console.WriteLine("hello3");
                // return Index(user.Id);

                // create a new cookie
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("MyCookie", "CookieValue", options);

                // return Redirect($"Home/Index/{user.Id}");

                return RedirectToAction("Index", "Home", new { userId = user.Id, area="" });
            }
            ModelState.AddModelError("", "Invalid username or password");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }


    [Authorize(Roles = "Admin")]
    public IActionResult Index(string userId)
    {
        Console.WriteLine("hello");
        var todoItems = _dbContext.TodoItems.ToList();
        ViewBag.UserId = userId;
        return View(todoItems);
    }

    public IActionResult Archived(string userId)
    {
        var todoItems = _dbContext.TodoItems.ToList();
        ViewBag.UserId = userId;
        return View(todoItems);
    }

    public IActionResult Completed(string userId)
    {
        var todoItems = _dbContext.TodoItems.ToList();
        ViewBag.UserId = userId;
        return View(todoItems);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoItemCreateDto todoItemCreateDto)
    {
        var response = await _dataClient.SendTodoItemFromWebToDb(todoItemCreateDto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(HomeController.Index), "Home", new { userId = todoItemCreateDto.UserId });
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