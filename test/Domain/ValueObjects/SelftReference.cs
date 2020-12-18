using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.ValueObjects
{
    class SelfReference : ValueObject<SelfReference>
    {
        public SelfReference()
        {
        }

        public SelfReference(SelfReference value)
        {
            Value = value;
        }

        public SelfReference Value { get; set; }
    }
}
