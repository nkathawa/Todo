using Todo.Models;

namespace Todo.Data
{
    public class TodoRepo : ITodoRepo
    {
        private readonly AppDbContext _context;

        public TodoRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateTodoItem(TodoItem todoItem, string userId)
        {
            todoItem = AddUserIdToTodoItem(todoItem, userId);
            if (todoItem == null)
            {
                throw new ArgumentNullException(nameof(todoItem));
            }

            _context.TodoItems.Add(todoItem);
        }

        private TodoItem AddUserIdToTodoItem(TodoItem todoItem, string userId)
        {
            todoItem.UserId = userId;
            return todoItem;
        }

        public IEnumerable<TodoItem> GetAllTodoItems()
        {
            return _context.TodoItems.ToList();
        }

        public TodoItem GetTodoItemById(int id)
        {
            return _context.TodoItems.FirstOrDefault(t => t.Id == id);
        }

        public void DeleteTodoItem(TodoItem item)
        {
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}