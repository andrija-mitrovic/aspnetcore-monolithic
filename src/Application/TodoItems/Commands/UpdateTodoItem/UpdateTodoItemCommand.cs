using MediatR;

namespace Application.TodoItems.Commands.UpdateTodoItem
{
    public class UpdateTodoItemCommand : IRequest
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public bool Done { get; set; }
    }
}
