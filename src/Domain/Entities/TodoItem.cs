﻿using Domain.Common;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities
{
    public class TodoItem : BaseAuditableEntity
    {
        public int ListId { get; set; }
        public string? Title { get; set; }
        public string? Note { get; set; }
        public PriorityLevel Priority { get; set; }
        public DateTime? Reminder { get; set; }
        public TodoList List { get; set; } = null!;

        private bool _done;

        public bool Done
        {
            get => _done;
            set
            {
                if (value == true && _done == false)
                {
                    AddDomainEvent(new TodoItemCompletedEvent(this));
                }

                _done = value;
            }
        }
    }
}
