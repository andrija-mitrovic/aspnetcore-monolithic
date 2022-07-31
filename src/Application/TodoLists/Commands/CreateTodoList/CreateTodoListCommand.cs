using MediatR;

namespace Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommand : IRequest<int>
    {
        public string? Title { get; set; }
    }
}
