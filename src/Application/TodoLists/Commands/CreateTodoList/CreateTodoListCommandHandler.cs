using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateTodoListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = new TodoList();

            todoList.Title = request.Title;

            _context.TodoLists.Add(todoList);

            await _context.SaveChangesAsync(cancellationToken);

            return todoList.Id;
        }
    }
}
