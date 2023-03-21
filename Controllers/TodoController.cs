using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Dtos;
using Todo.Models;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITodoRepo _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(IMapper mapper, ITodoRepo repo, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _repo = repo;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItemReadDto>> GetTodoItems()
        {
            Console.WriteLine("--> getting items...");
            var todoItems = _repo.GetAllTodoItems();
            return Ok(_mapper.Map<IEnumerable<TodoItemReadDto>>(todoItems));
        }

        [HttpGet("userItems/{userId}", Name = "GetTodoItemsByUser")]
        public ActionResult<IEnumerable<TodoItemReadDto>> GetTodoItemsByUser(string userId)
        {
            Console.WriteLine("--> getting items...");
            var todoItems = _repo.GetAllTodoItems(userId);
            return Ok(_mapper.Map<IEnumerable<TodoItemReadDto>>(todoItems));
        }

        [HttpGet("{id}", Name = "GetTodoItemById")]
        public ActionResult<TodoItemReadDto> GetTodoItemById(int id)
        {
            Console.WriteLine("--> getting an item...");
            var todoItem = _repo.GetTodoItemById(id);
            return Ok(_mapper.Map<TodoItemReadDto>(todoItem));
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemCreateDto>> CreateNewTodoItem(TodoItemCreateDto todoItemCreateDto)
        {
            Console.WriteLine("--> creating an item...");

            var todoItem = _mapper.Map<TodoItem>(todoItemCreateDto);
            var user = await _userManager.FindByNameAsync("Admin");
            if (user != null)
            {
                var userId = user.Id;

                _repo.CreateTodoItem(todoItem, userId);
                _repo.SaveChanges();
                var todoItemReadDto = _mapper.Map<TodoItemReadDto>(todoItem);

                return CreatedAtRoute(nameof(GetTodoItemById), new { Id = todoItemReadDto.Id }, todoItemReadDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<TodoItemCreateDto> DeleteTodoItem(int id)
        {
            Console.WriteLine("--> deleting an item...");

            var itemToDelete = _repo.GetTodoItemById(id);
            if (itemToDelete == null)
            {
                return NotFound();
            }
            _repo.DeleteTodoItem(itemToDelete);
            _repo.SaveChanges();
            return Ok();
        }
    }
}