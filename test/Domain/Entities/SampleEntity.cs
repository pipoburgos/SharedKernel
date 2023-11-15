using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Tests.Entities;

/// <inheritdoc />
/// <summary>
/// A sample entity for unit testing
/// </summary>
public class SampleEntity : Entity<Guid>
{
    public SampleEntity() { }

    public SampleEntity(Guid id) : base(id) { }

    public string SampleProperty { get; set; } = null!;
}