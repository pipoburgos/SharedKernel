using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Integration.Tests.Cqrs.Commands
{
    internal class SampleCommand : ICommandRequest
    {
        public SampleCommand(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
