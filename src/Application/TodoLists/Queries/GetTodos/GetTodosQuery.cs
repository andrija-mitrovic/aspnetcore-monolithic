using MediatR;

namespace Application.TodoLists.Queries.GetTodos
{
    public class GetTodosQuery : IRequest<TodosVm>
    {
    }
}
