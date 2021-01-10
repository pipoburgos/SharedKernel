﻿using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Infraestructure.Tests.Cqrs.Commands
{
    internal class SampleCommandWithResponse : ICommandRequest<int>
    {
        public SampleCommandWithResponse(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
