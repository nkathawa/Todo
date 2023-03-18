using Todo.Models;

namespace Todo.Data
{
    public interface ITodoRepo
    {
        bool SaveChanges();

        IEnumerable<TodoItem> GetAllTodoItems();
        TodoItem GetTodoItemById(int id);
        void CreateTodoItem(TodoItem todoItem, string userId);
    }
}