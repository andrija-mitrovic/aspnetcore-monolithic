using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TodoLists.Commands.DeleteTodoList
{
    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTodoListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _context.TodoLists.Where(x => x.Id == request.Id).SingleOrDefaultAsync(cancellationToken);

            if (todoList == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            _context.TodoLists.Remove(todoList);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
