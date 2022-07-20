namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        private readonly List<BaseEvent> _domainEvents = new();
    }
}
