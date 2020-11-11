using System;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace SharedKernel.Application.Tests.Reflection
{
    internal class TestObjectNoSetters
    {
        public string Name { get; }

        public DateTime Created { get; }

        public DateTime? Birthday { get; }

        public Guid Id { get; }

        public Guid? AdminId { get; }

        public int Age { get; }

        public double? Latitude { get; }
    }
}