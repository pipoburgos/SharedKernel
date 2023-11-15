using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.Entities;

public class CarLicensePlateGuid : ValueObject<CarLicensePlateGuid>
{
    protected CarLicensePlateGuid() { }

    public static CarLicensePlateGuid Create(Guid license)
    {
        return new CarLicensePlateGuid
        {
            Value = license
        };
    }

    public Guid Value { get; private set; }
}