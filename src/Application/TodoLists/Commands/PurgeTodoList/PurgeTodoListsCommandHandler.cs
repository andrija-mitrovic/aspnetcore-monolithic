﻿using Application.Common.Interfaces;
using MediatR;

namespace Application.TodoLists.Commands.PurgeTodoList
{
    public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
    {
        private readonly IApplicationDbContext _context;

        public PurgeTodoListsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
        {
            _context.TodoLists.RemoveRange(_context.TodoLists);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
