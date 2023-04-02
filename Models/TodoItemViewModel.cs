using Todo.Models;

public class TodoItemViewModel
{
    public TodoItemViewModel(IEnumerable<TodoItem> todoItems, string userId, int completedStatus, int archivedStatus)
    {
        TodoItems = todoItems;
        UserId = userId;
        CompletedStatus = completedStatus;
        ArchivedStatus = archivedStatus;
    }

    public IEnumerable<TodoItem> TodoItems { get; set; }
    public string UserId { get; set; }
    public int CompletedStatus { get; set; }
    public int ArchivedStatus { get; set; }
}
