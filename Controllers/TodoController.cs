using AutoMapper;
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

        public TodoController(IMapper mapper, ITodoRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItemReadDto>> GetTodoItems()
        {
            Console.WriteLine("--> getting items...");
            var todoItems = _repo.GetAllTodoItems();
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
        public ActionResult<TodoItemCreateDto> CreateNewTodoItem(TodoItemCreateDto todoItemCreateDto)
        {
            Console.WriteLine("--> creating an item...");

            var todoItem = _mapper.Map<TodoItem>(todoItemCreateDto);
            _repo.CreateTodoItem(todoItem);
            _repo.SaveChanges();
            var todoItemReadDto = _mapper.Map<TodoItemReadDto>(todoItem);

            return CreatedAtRoute(nameof(GetTodoItemById), new { Id = todoItemReadDto.Id }, todoItemReadDto);
        }
    }
}