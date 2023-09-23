namespace SharedKernel.Domain.Tests.Entities
{
    public class CartWithIdOfValueObjectTests
    {
        [Fact]
        public void TestSameLicense()
        {
            var license1 = CarLicensePlate.Create("2151-AAA");
            var car1 = new Car(license1);

            var license2 = CarLicensePlate.Create("2151-AAA");
            var car2 = new Car(license2);

            (car1 == car2).Should().Be(true);
        }

        [Fact]
        public void TestDistinctLicense()
        {
            var license1 = CarLicensePlate.Create("2151-AAA");
            var car1 = new Car(license1);

            var license2 = CarLicensePlate.Create("5555-AAA");
            var car2 = new Car(license2);

            (car1 == car2).Should().Be(false);
        }

        [Fact]
        public void TestSameLicenseGuid()
        {
            var license1 = CarLicensePlateGuid.Create(new Guid("00000000-0000-0000-0000-000000000001"));
            var car1 = new CarGuid(license1);

            var license2 = CarLicensePlateGuid.Create(new Guid("00000000-0000-0000-0000-000000000001"));
            var car2 = new CarGuid(license2);

            (car1 == car2).Should().Be(true);
        }

        [Fact]
        public void TestDistinctLicenseGuid()
        {
            var license1 = CarLicensePlateGuid.Create(new Guid("00000000-0000-0000-0000-000000000001"));
            var car1 = new CarGuid(license1);

            var license2 = CarLicensePlateGuid.Create(new Guid("10000000-0000-0000-0000-000000000000"));
            var car2 = new CarGuid(license2);

            (car1 == car2).Should().Be(false);
        }
    }
}
