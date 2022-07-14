using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Tests.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// A sample entity for unit testing
    /// </summary>
    public class CarGuid : Entity<CarLicensePlateGuid>
    {
        public CarGuid() { }

        public CarGuid(CarLicensePlateGuid id) : base(id) { }

        public string Color { get; set; }
    }
}
