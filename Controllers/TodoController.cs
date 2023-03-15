using Microsoft.AspNetCore.Mvc;
using Todo.Dtos;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        public TodoController()
        {
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItemReadDto>> GetTodoItems()
        {
            Console.WriteLine("--> getting items...");
            return Ok();
        }

        [HttpGet("{id}", Name = "GetTodoItemById")]
        public ActionResult<TodoItemReadDto> GetTodoItemById(int id)
        {
            Console.WriteLine("--> getting an item...");
            return Ok();
        }

        [HttpPost]
        public ActionResult<TodoItemCreateDto> CreateNewTodoItem(TodoItemReadDto todoItemReadDto)
        {
            Console.WriteLine("--> creating an item...");
            return Ok();
        }
    }
}