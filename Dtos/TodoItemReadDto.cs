using Todo.Enums;

namespace Todo.Dtos
{
    public class TodoItemReadDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string UserId { get; set; }
        
        public StatusType Status { get; set; }
    }
}