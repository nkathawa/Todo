using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    private readonly ITodoRepo _repo;

    public HomeController(ILogger<HomeController> logger,
        ITodoDataClient dataClient,
        AppDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        ITodoRepo repo)
    {
        _logger = logger;
        _dataClient = dataClient;
        _dbContext = dbContext;
        _userManager = userManager;
        _repo = repo;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction(nameof(HomeController.Index), "Home", new { userId = user.Id });
            }
            ModelState.AddModelError("", "Invalid username or password");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Signup()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signup(LoginViewModel model)
    {            
        if (_userManager.FindByNameAsync(model.Username).Result == null)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Username,
                Email = "acb@123.com",
                EmailConfirmed = true,
                FirstName = "Jim",
                LastName = "Cricket",
                Id = "qwerty"
            };

            IdentityResult result = _userManager.CreateAsync(user, "AdminPassword123!").Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
        _repo.SaveChanges();
        return RedirectToAction(nameof(HomeController.Index), "Home", new { userId = "qwerty" });
    }

    public IActionResult Index(string userId)
    {
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
        var userId = Request.Form["userId"];
        todoItemCreateDto.UserId = userId;
        var response = await _dataClient.SendTodoItemFromWebToDb(todoItemCreateDto);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(HomeController.Index), "Home", new { userId = userId });
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