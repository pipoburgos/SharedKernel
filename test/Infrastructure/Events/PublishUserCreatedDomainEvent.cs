namespace SharedKernel.Integration.Tests.Events
{
    public class PublishUserCreatedDomainEvent
    {
        private readonly object _lock;

        public PublishUserCreatedDomainEvent()
        {
            Users = new List<Guid>();
            _lock = new object();
        }

        public List<Guid> Users { get; }

        public int Total { get; private set; }

        public void SetUser(Guid id)
        {
            lock (_lock)
            {
                Users.Add(id);
            }
        }

        public void SumTotal()
        {
            lock (_lock)
            {
                Total++;
            }

        }
    }
}
