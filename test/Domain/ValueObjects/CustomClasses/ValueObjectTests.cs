using FluentAssertions;
using Xunit;

namespace SharedKernel.Domain.Tests.ValueObjects.CustomClasses;

public class ValueObjectTests
{
    [Fact]
    public void SameHouseCityWithSameUsers()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var user1 = new User(3, new List<Address> { address1, address2 });
        var user2 = new User(7, new List<Address> { address2, address1 });
        var house1 = new House(new List<User> { user1, user2 });
        var house2 = new House(new List<User> { user2, user1 });

        house1.Should().Be(house2);
    }

    [Fact]
    public void SameHouseCityWithDistintUsers()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var user1 = new User(1, new List<Address> { address1, address2 });
        var user2 = new User(2, new List<Address> { address2, address1 });
        var user3 = new User(3, new List<Address> { address2, address1 });
        var house1 = new House(new List<User> { user1, user2 });
        var house2 = new House(new List<User> { user2, user3 });

        house1.Should().NotBe(house2);
    }

    [Fact]
    public void SameCityWithSameAddress()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var user1 = new City(new List<Address> { address1, address2 });
        var user2 = new City(new List<Address> { address2, address1 });

        user1.Should().Be(user2);
    }

    [Fact]
    public void CitiesWithDistinctAddress()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address3 = new Address("streetLine1", "streetLine3", "city", "zipcode");
        var city1 = new City(new List<Address> { address1, address2 });
        var city2 = new City(new List<Address> { address2, address3 });

        city1.Should().NotBe(city2);
    }

    [Fact]
    public void SameUserWithSameAddress()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var user1 = new User(333, new List<Address> { address1, address2 });
        var user2 = new User(333, new List<Address> { address2, address1 });

        user1.Should().Be(user2);
    }

    [Fact]
    public void IdenticalDataEqualsIsTrueTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");

        //Assert
        address1.Should().Be(address2);
    }

    [Fact]
    public void IdenticalDataEqualOperatorIsTrueTest()
    {
        //Arraneg
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");

        //Act
        var resultEquals = address1 == address2;
        var resultEqualsSimetric = address2 == address1;

        //Assert
        resultEquals.Should().BeTrue();
        resultEqualsSimetric.Should().BeTrue();
    }

    [Fact]
    public void IdenticalDataIsNotEqualOperatorIsFalseTest()
    {
        //Arraneg
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");

        //Act
        var resultEquals = address1 != address2;
        var resultEqualsSimetric = address2 != address1;
        // ReSharper disable once EqualExpressionComparison
        //var resultEqualsOnThis = address1 != address1;

        //Assert
        Assert.False(resultEquals);
        Assert.False(resultEqualsSimetric);
        //Assert.False(resultEqualsOnThis);
    }

    [Fact]
    public void DiferentDataEqualsIsFalseTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

        //Act
        var result = address1.Equals(address2);
        var resultSimetric = address2.Equals(address1);

        //Assert
        Assert.False(result);
        Assert.False(resultSimetric);
    }

    [Fact]
    public void DiferentDataIsNotEqualOperatorIsTrueTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

        //Act
        var result = address1 != address2;
        var resultSimetric = address2 != address1;

        //Assert
        Assert.True(result);
        Assert.True(resultSimetric);
    }

    [Fact]
    public void DiferentDataEqualOperatorIsFalseTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
        var address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

        //Act
        var result = address1 == address2;
        var resultSimetric = address2 == address1;

        //Assert
        Assert.False(result);
        Assert.False(resultSimetric);
    }

    [Fact]
    public void SameDataInDiferentPropertiesIsEqualsFalseTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", default!, default!);
        var address2 = new Address("streetLine2", "streetLine1", default!, default!);

        //Act
        var result = address1.Equals(address2);


        //Assert
        Assert.False(result);
    }

    [Fact]
    public void SameDataInDiferentPropertiesEqualOperatorFalseTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", default!, default!);
        var address2 = new Address("streetLine2", "streetLine1", default!, default!);

        //Act
        var result = address1 == address2;


        //Assert
        Assert.False(result);
    }

    [Fact]
    public void NullableValueObjectEqualTrueTest()
    {
        //Arrange
        Address address1 = default!;
        Address address2 = default!;

        //Act
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        var result = address1 == address2;


        //Assert
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        result.Should().BeTrue();
    }

    [Fact]
    public void NullablePropertiesValueObjectEqualTrueTest()
    {
        //Arrange
        var address1 = new Address(default!, default!, default!, default!);
        var address2 = new Address(default!, default!, default!, default!);

        //Act
        var result = address1 == address2;


        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void DiferentDataInDiferentPropertiesProduceDiferentHashCodeTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", default!, default!);
        var address2 = new Address("streetLine2", "streetLine1", default!, default!);

        //Act
        var address1HashCode = address1.GetHashCode();
        var address2HashCode = address2.GetHashCode();


        //Assert
        Assert.NotEqual(address1HashCode, address2HashCode);
    }

    [Fact]
    public void SameDataInDiferentPropertiesProduceDiferentHashCodeTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", default!, default!, "streetLine1");
        var address2 = new Address(default!, "streetLine1", "streetLine1", default!);

        //Act
        var address1HashCode = address1.GetHashCode();
        var address2HashCode = address2.GetHashCode();


        //Assert
        Assert.NotEqual(address1HashCode, address2HashCode);
    }

    [Fact]
    public void SameReferenceEqualsTrueTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", default!, default!, "streetLine1");
        var address2 = address1;


        //Act
        if (!address1.Equals(address2))
            Assert.Fail("error");

        if (!(address1 == address2))
            Assert.Fail("error");

    }

    [Fact]
    public void SameDataSameHashCodeTest()
    {
        //Arrange
        var address1 = new Address("streetLine1", "streetLine2", default!, default!);
        var address2 = new Address("streetLine1", "streetLine2", default!, default!);

        //Act
        var address1HashCode = address1.GetHashCode();
        var address2HashCode = address2.GetHashCode();


        //Assert
        Assert.Equal(address1HashCode, address2HashCode);
    }

    [Fact]
    public void SelfReferenceNotProduceInfiniteLoop()
    {
        //Arrange
        SelfReference bReference = default!;
        // ReSharper disable once ExpressionIsAlwaysNull
        var aReference = new SelfReference(bReference);
        bReference = new SelfReference(aReference);

        //Act

        //Assert
        aReference.Should().NotBe(bReference);
    }

    [Fact]
    public void AllNullPropertiesEquals()
    {
        //Arrange
        var address1 = new Address(default!, default!, default!, default!);
        var address2 = new Address(default!, default!, default!, default!);

        //Act

        //Assert
        Assert.Equal(address1, address2);
    }

    [Fact]
    public void SameListAreEquals()
    {
        //Arrange
        var integers1 = new Integers(new List<int> { 1, 3 });
        var integers2 = new Integers(new List<int> { 1, 3 });

        //Act

        //Assert
        Assert.True(integers1.Equals(integers2));
        Assert.Equal(integers1, integers2);
    }


    [Fact]
    public void SameListDistinctOrderAreEquals()
    {
        //Arrange
        var integers1 = new Integers(new List<int> { 3, 1 });
        var integers2 = new Integers(new List<int> { 1, 3 });

        //Act

        //Assert
        Assert.True(integers1.Equals(integers2));
        Assert.Equal(integers1, integers2);
    }

    [Fact]
    public void IdenticalDataIsNotEqualOperatorIsFalseTestWithList()
    {
        //Arraneg
        var integers1 = new Integers(new List<int> { 3, 1 });
        var integers2 = new Integers(new List<int> { 1, 3 });

        //Act
        var resultEquals = integers1 != integers2;
        var resultEqualsSimetric = integers2 != integers1;
        // ReSharper disable once EqualExpressionComparison
        //var resultEqualsOnThis = address1 != address1;

        //Assert
        Assert.False(resultEquals);
        Assert.False(resultEqualsSimetric);
        //Assert.False(resultEqualsOnThis);
    }
}
