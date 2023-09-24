
// ReSharper disable HeuristicUnreachableCode
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace SharedKernel.Domain.Tests.Entities
{
    public class EntityTests
    {
        [Fact]
        public void SameIdentityProduceEqualsTrueTest()
        {
            //Arrange
            var id = Guid.NewGuid();

            var entityLeft = new SampleEntity(id);
            var entityRight = new SampleEntity(id);

            //Act
            var resultOnEquals = entityLeft.Equals(entityRight);
            var resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.True(resultOnEquals);
            Assert.True(resultOnOperator);
        }

        [Fact]
        public void DiferentIdProduceEqualsFalseTest()
        {
            //Arrange
            var entityLeft = new SampleEntity(Guid.NewGuid());
            var entityRight = new SampleEntity(Guid.NewGuid());


            //Act
            var resultOnEquals = entityLeft.Equals(entityRight);
            var resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.False(resultOnEquals);
            Assert.False(resultOnOperator);
        }

        [Fact]
        public void CompareUsingEqualsOperatorsAndNullOperandsTest()
        {
            //Arrange

            SampleEntity entityLeft = default!;
            var entityRight = new SampleEntity();

            //entityRight.GenerateNewIdentity();

            //Act
            if (entityLeft != default!) //this perform ==(left,right)
                Assert.Fail("error");

            if (entityRight == null)//this perform !=(left,right)
                Assert.Fail("error");

            entityRight = null;

            //Act
            if (entityLeft != entityRight)//this perform ==(left,right)
                Assert.Fail("error");

            if (entityLeft != entityRight)//this perform !=(left,right)
                Assert.Fail("error");
        }

        [Fact]
        public void CompareTheSameReferenceReturnTrueTest()
        {
            //Arrange
            var entityLeft = new SampleEntity();
            var entityRight = entityLeft;


            //Act
            if (!entityLeft.Equals(entityRight))
                Assert.Fail("error");

            if (entityLeft != entityRight)
                Assert.Fail("error");
        }

        [Fact]
        public void CompareWhenLeftIsNullAndRightIsNullReturnFalseTest()
        {
            //Arrange
            SampleEntity entityLeft = default!;
            var entityRight = new SampleEntity();

            //entityRight.GenerateNewIdentity();

            //Act
            if (entityLeft != default!)//this perform ==(left,right)
                Assert.Fail("error");

            if (entityRight == null)//this perform !=(left,right)
                Assert.Fail("error");
        }
    }
}
