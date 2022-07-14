using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Tests.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// A sample entity for unit testing
    /// </summary>
    public class Car : Entity<CarLicensePlate>
    {
        public Car() { }

        public Car(CarLicensePlate id) : base(id) { }

        public string Color { get; set; }
    }
}
