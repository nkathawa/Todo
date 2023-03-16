using AutoMapper;
using Todo.Dtos;
using Todo.Models;

namespace Todo.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            // Source -> Target
            // Need to explicitly add the opposite mapping if desired
            CreateMap<TodoItem, TodoItemReadDto>();

            CreateMap<TodoItemCreateDto, TodoItem>();
        }
    }
}