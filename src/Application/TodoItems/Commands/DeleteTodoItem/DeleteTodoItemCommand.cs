using MediatR;

namespace Application.TodoItems.Commands.DeleteTodoItem
{
    public record DeleteTodoItemCommand(int Id) : IRequest;
}
