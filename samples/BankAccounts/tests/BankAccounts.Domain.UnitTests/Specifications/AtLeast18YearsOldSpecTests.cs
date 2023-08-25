using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Specifications;

namespace BankAccounts.Domain.Tests.Specifications
{
    public class AtLeast18YearsOldSpecTests
    {
        [Fact]
        public void IsbirthDateSameAsToday()
        {
            // Arrange
            var today = new DateTime(1990, 2, 2);
            var birthdate = today;
            var user = User.Create(Guid.NewGuid(), "Roberto", "Fernández", birthdate).Value;

            // Act
            var isAtLeast18YearsOld = new AtLeast18YearsOldSpec(today).SatisfiedBy().Compile()(user);

            // Assert
            isAtLeast18YearsOld.Should().BeFalse();
        }

        [Fact]
        public void Is18YearsOld()
        {
            // Arrange
            var today = new DateTime(1990, 2, 2);
            var birthdate = today.AddYears(-18);
            var user = User.Create(Guid.NewGuid(), "Roberto", "Fernández", birthdate).Value;

            // Act
            var isAtLeast18YearsOld = new AtLeast18YearsOldSpec(today).SatisfiedBy().Compile()(user);

            // Assert
            isAtLeast18YearsOld.Should().BeTrue();
        }

        [Fact]
        public void Is18YearsOldAnADay()
        {
            // Arrange
            var today = new DateTime(1990, 2, 2);
            var birthdate = today.AddYears(-18).AddDays(-1);
            var user = User.Create(Guid.NewGuid(), "Roberto", "Fernández", birthdate).Value;

            // Act
            var isAtLeast18YearsOld = new AtLeast18YearsOldSpec(today).SatisfiedBy().Compile()(user);

            // Assert
            isAtLeast18YearsOld.Should().BeTrue();
        }

        [Fact]
        public void Is18LessADay()
        {
            // Arrange
            var today = new DateTime(1990, 2, 2);
            var birthdate = today.AddYears(-18).AddDays(1);
            var user = User.Create(Guid.NewGuid(), "Roberto", "Fernández", birthdate).Value;

            // Act
            var isAtLeast18YearsOld = new AtLeast18YearsOldSpec(today).SatisfiedBy().Compile()(user);

            // Assert
            isAtLeast18YearsOld.Should().BeFalse();
        }

        [Fact]
        public void Is18LessADay40YearsOld()
        {
            // Arrange
            var today = new DateTime(2022, 2, 14);
            var birthdate = new DateTime(1980, 2, 24);
            var user = User.Create(Guid.NewGuid(), "Roberto", "Fernández", birthdate).Value;

            // Act
            var isAtLeast18YearsOld = new AtLeast18YearsOldSpec(today).SatisfiedBy().Compile()(user);

            // Assert
            isAtLeast18YearsOld.Should().BeTrue();
        }

    }
}
