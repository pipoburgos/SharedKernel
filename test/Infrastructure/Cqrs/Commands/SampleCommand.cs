using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Domain.Requests;
using System.Collections.Generic;

namespace SharedKernel.Integration.Tests.Cqrs.Commands;

internal class SampleCommand : CommandRequest
{
    public SampleCommand(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public override string GetUniqueName()
    {
        return "sampleCommand.";
    }

    public override Request FromPrimitives(Dictionary<string, string> body, string id, string occurredOn)
    {
        return new SampleCommand(int.Parse(body[nameof(Value)]));
    }
}
