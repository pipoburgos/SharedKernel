using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.Entities
{
    public class CarLicensePlate : ValueObject<CarLicensePlate>
    {
        protected CarLicensePlate() { }

        public static CarLicensePlate Create(string license)
        {
            return new CarLicensePlate
            {
                Value = license.PadLeft(8, '0')
            };
        }

        public string Value { get; private set; }
    }
}
