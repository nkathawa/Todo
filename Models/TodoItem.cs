using System.ComponentModel.DataAnnotations;
using Todo.Enums;

namespace Todo.Models
{
    public class TodoItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public StatusType Status { get; set; }
    }
}