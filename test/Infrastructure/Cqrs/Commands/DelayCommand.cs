using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Integration.Tests.Cqrs.Commands;

internal class DelayCommand : ICommandRequest
{
    public DelayCommand(int seconds)
    {
        Seconds = seconds;
    }

    public int Seconds { get; }
}