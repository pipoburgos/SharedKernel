using System.Collections.Generic;

namespace SharedKernel.Domain.Tests.ValueObjects.Records;

public record City(IEnumerable<Address> Addresses);