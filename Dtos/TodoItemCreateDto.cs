using Todo.Enums;

namespace Todo.Dtos
{
    public class TodoItemCreateDto
    {
        public string Text { get; set; }

        public string UserId { get; set; }
        
        public StatusType Status { get; set; }
    }
}