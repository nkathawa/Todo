using Todo.Enums;

namespace Todo.Dtos
{
    public class TodoItemCreateDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
        
        public StatusType Status { get; set; }
    }
}