using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.TodoItems.Commands.CreateTodoItem
{
    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateTodoItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoItem = new TodoItem
            {
                ListId = request.ListId,
                Title = request.Title,
                Done = false
            };

            todoItem.AddDomainEvent(new TodoItemCreatedEvent(todoItem));

            _context.TodoItems.Add(todoItem);

            await _context.SaveChangesAsync(cancellationToken);

            return todoItem.Id;
        }
    }
}
