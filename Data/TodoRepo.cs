using Todo.Dtos;
using Todo.Enums;
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

            var todoItems = _context.TodoItems;
            if (todoItems != null)
            {
                todoItems.Add(todoItem);
            }
        }

        private TodoItem AddUserIdToTodoItem(TodoItem todoItem, string userId)
        {
            todoItem.UserId = userId;
            return todoItem;
        }

        public IEnumerable<TodoItem> GetAllTodoItems(string userId = null)
        {
            if (userId != null)
            {
                var todoItems = _context.TodoItems;
                if (todoItems != null)
                {
                    return todoItems
                        .Where(x => x.UserId == userId)
                        .ToList();
                }
                else
                {
                    return new List<TodoItem>();
                }
            }
            else
            {
                var todoItems = _context.TodoItems;
                if (todoItems != null)
                {
                    return todoItems.ToList();
                }
                else
                {
                    return new List<TodoItem>();
                }
            }
        }

        public TodoItem? GetTodoItemById(int id)
        {
            var todoItems = _context.TodoItems;
            if (todoItems != null)
            {
                return todoItems.FirstOrDefault(t => t.Id == id);
            }
            return null;
        }

        public void DeleteTodoItem(TodoItem item)
        {
            var todoItems = _context.TodoItems;
            if (todoItems != null)
            {
                todoItems.Remove(item);
                _context.SaveChanges();
            }
        }

        public void UpdateTodoItem(int id, TodoItem item)
        {
            var itemToUpdate = GetTodoItemById(id);
            if (itemToUpdate != null)
            {
                itemToUpdate.Status = item.Status;
                itemToUpdate.Title = item.Title;
                itemToUpdate.Description = item.Description;
                _context.SaveChanges();
            }
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public IEnumerable<TodoItem> GetAllUserTodoItemsByStatus(string userId, StatusType status)
        {
            var todoItems = _context.TodoItems;
            if (todoItems != null)
            {
                return todoItems
                    .Where(x => x.UserId == userId)
                    .Where(x => x.Status == status)
                    .ToList();
            }
            else
            {
                return new List<TodoItem>();
            }
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}