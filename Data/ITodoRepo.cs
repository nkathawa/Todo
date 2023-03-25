using Todo.Dtos;
using Todo.Models;

namespace Todo.Data
{
    public interface ITodoRepo
    {
        bool SaveChanges();

        IEnumerable<TodoItem> GetAllTodoItems(string userId = null);
        TodoItem GetTodoItemById(int id);
        void CreateTodoItem(TodoItem todoItem, string userId);

        void DeleteTodoItem(TodoItem item);

        void UpdateTodoItem(int id, TodoItem item);
    }
}