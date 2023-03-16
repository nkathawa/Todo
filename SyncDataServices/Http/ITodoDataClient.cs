using Todo.Dtos;

namespace Todo.SyncDataServices.Http
{
    public interface ITodoDataClient
    {
        Task<HttpResponseMessage> SendTodoItemFromWebToDb(TodoItemCreateDto createDto);
    }
}