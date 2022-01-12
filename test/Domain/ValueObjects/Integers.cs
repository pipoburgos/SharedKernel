using SharedKernel.Domain.ValueObjects;
using System.Collections.Generic;

namespace SharedKernel.Domain.Tests.ValueObjects
{
    internal class Integers : ValueObject<Integers>
    {
        public List<int> Ints { get; set; }
    }
}
