using MediatR;

namespace Application.TodoLists.Commands.DeleteTodoList
{
    public record DeleteTodoListCommand(int Id) : IRequest;
}
