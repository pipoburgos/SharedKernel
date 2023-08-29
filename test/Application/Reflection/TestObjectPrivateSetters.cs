namespace SharedKernel.Application.Tests.Reflection
{
    internal class TestObjectPrivateSetters
    {
        public string Name { get; private set; } = null!;

        public DateTime Created { get; private set; }

        public DateTime? Birthday { get; private set; }

        public Guid Id { get; private set; }

        public Guid? AdminId { get; private set; }

        public int Age { get; private set; }

        public double? Latitude { get; private set; }
    }
}