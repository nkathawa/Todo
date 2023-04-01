using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITodoRepo _repo;

        public UserController(ITodoRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            Console.WriteLine("--> getting users...");
            return Ok(_repo.GetAllUsers());
        }
    }
}