using Application.Common.Models;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    public class GetTodoItemsWithPaginationQuery : IRequest<PaginatedList<TodoItemBriefDto>>
    {
        public int ListId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
