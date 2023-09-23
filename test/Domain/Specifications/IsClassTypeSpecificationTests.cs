using SharedKernel.Domain.Specifications;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Domain.Tests.Specifications
{
    public class IsClassTypeSpecificationTests
    {
        [Fact]
        public void BooleanIsPrimary()
        {
            Test<bool>().Should().BeFalse();
        }

        [Fact]
        public void IntegerIsPrimary()
        {
            Test<int>().Should().BeFalse();
        }

        [Fact]
        public void ListIsPrimary()
        {
            Test<List<string>>().Should().BeFalse();
        }

        [Fact]
        public void StringIsPrimary()
        {
            Test<string>().Should().BeFalse();
        }

        [Fact]
        public void EnumIsPrimary()
        {
            Test<DayOfWeek>().Should().BeFalse();
        }

        [Fact]
        public void UserNotPrimary()
        {
            Test<User>().Should().BeTrue();
        }

        public bool Test<T>()
        {
            return new IsClassTypeSpecification<T>().SatisfiedBy().Compile()(default!);
        }
    }


}
