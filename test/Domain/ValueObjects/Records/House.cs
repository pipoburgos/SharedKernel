using System.Collections.Generic;

namespace SharedKernel.Domain.Tests.ValueObjects.Records;

public record House(IEnumerable<User> Users);