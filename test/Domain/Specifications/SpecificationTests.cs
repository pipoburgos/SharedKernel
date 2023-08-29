using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Domain.Tests.Entities;
using System.Linq.Expressions;
using Xunit;

// ReSharper disable JoinDeclarationAndInitializer

namespace SharedKernel.Domain.Tests.Specifications
{
    /// <summary>
    /// Summary description for SpecificationTests
    /// </summary>
    public class SpecificationTests
    {

        //[Fact]
        //public void CreateNewDirectSpecificationTest()
        //{
        //    //Arrange
        //    DirectSpecification<SampleEntity> adHocSpecification;
        //    Expression<Func<SampleEntity, bool>> spec = s => s.Id == Guid.NewGuid();

        //    //Act
        //    adHocSpecification = new DirectSpecification<SampleEntity>(spec);

        //    //Assert
        //    Assert.Equal(new PrivateObject(adHocSpecification).GetField("_matchingCriteria"), spec);
        //}

        [Fact]
        public void CreateDirectSpecificationNullSpecThrowArgumentNullExceptionTest()
        {
            //Act
            Assert.Throws<ArgumentNullException>(() => new DirectSpecification<SampleEntity>(default!));
        }

        [Fact]
        public void CreateAndSpecificationTest()
        {
            //Arrange
            var identifier = Guid.NewGuid();
            var leftAdHocSpecification = new DirectSpecification<SampleEntity>(s => s.Id == identifier);
            var rightAdHocSpecification = new DirectSpecification<SampleEntity>(s => s.SampleProperty.Length > 2);

            //Act
            var composite = new AndSpecification<SampleEntity>(leftAdHocSpecification, rightAdHocSpecification);

            //Assert
            Assert.NotNull(composite.SatisfiedBy());
            Assert.Equal(leftAdHocSpecification, composite.LeftSideSpecification);
            Assert.Equal(rightAdHocSpecification, composite.RightSideSpecification);

            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity(identifier) { SampleProperty = "1" };
            var sampleB = new SampleEntity(identifier) { SampleProperty = "the sample property" };

            list.AddRange(new[] { sampleA, sampleB });


            var result = list.AsQueryable().Where(composite.SatisfiedBy()).ToList();

            Assert.True(result.Count == 1);
        }

        [Fact]
        public void CreateOrSpecificationTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            var composite = new OrSpecification<SampleEntity>(leftAdHocSpecification, rightAdHocSpecification);

            //Assert
            Assert.NotNull(composite.SatisfiedBy());
            Assert.Equal(leftAdHocSpecification, composite.LeftSideSpecification);
            Assert.Equal(rightAdHocSpecification, composite.RightSideSpecification);

            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity(identifier) { SampleProperty = "1" };

            var sampleB = new SampleEntity(Guid.NewGuid()) { SampleProperty = "the sample property" };

            list.AddRange(new[] { sampleA, sampleB });


            var result = list.AsQueryable().Where(composite.SatisfiedBy()).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void CreateAndSpecificationNullLeftSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            Assert.Throws<ArgumentNullException>(() => new AndSpecification<SampleEntity>(default!, rightAdHocSpecification));
        }

        [Fact]
        public void CreateAndSpecificationNullRightSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;

            Expression<Func<SampleEntity, bool>> leftSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);

            //Act
            Assert.Throws<ArgumentNullException>(() => new AndSpecification<SampleEntity>(leftAdHocSpecification, default!));
        }

        [Fact]
        public void CreateOrSpecificationNullLeftSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            Assert.Throws<ArgumentNullException>(() => new OrSpecification<SampleEntity>(default!, rightAdHocSpecification));
        }

        [Fact]
        public void CreateOrSpecificationNullRightSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;

            Expression<Func<SampleEntity, bool>> leftSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);

            //Act
            Assert.Throws<ArgumentNullException>(() => new OrSpecification<SampleEntity>(leftAdHocSpecification, default!));
        }

        [Fact]
        public void UseSpecificationLogicAndOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            ISpecification<SampleEntity> andSpec = leftAdHocSpecification & rightAdHocSpecification;

            //Assert

            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity(identifier) { SampleProperty = "1" };

            var sampleB = new SampleEntity(Guid.NewGuid()) { SampleProperty = "the sample property" };

            var sampleC = new SampleEntity(identifier) { SampleProperty = "the sample property" };

            list.AddRange(new[] { sampleA, sampleB, sampleC });

            var result = list.AsQueryable().Where(andSpec.SatisfiedBy()).ToList();

            Assert.True(result.Count == 1);
        }

        [Fact]
        public void UseSpecificationAndOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            ISpecification<SampleEntity> andSpec = leftAdHocSpecification && rightAdHocSpecification;

            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity(identifier) { SampleProperty = "1" };

            var sampleB = new SampleEntity(Guid.NewGuid()) { SampleProperty = "the sample property" };

            var sampleC = new SampleEntity(identifier) { SampleProperty = "the sample property" };

            list.AddRange(new[] { sampleA, sampleB, sampleC });


            var result = list.AsQueryable().Where(andSpec.SatisfiedBy()).ToList();

            Assert.True(result.Count == 1);
        }

        [Fact]
        public void UseSpecificationBitwiseOrOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            var orSpec = leftAdHocSpecification | rightAdHocSpecification;


            //Assert
            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity(identifier) { SampleProperty = "1" };

            var sampleB = new SampleEntity(Guid.NewGuid()) { SampleProperty = "the sample property" };

            list.AddRange(new[] { sampleA, sampleB });

            var result = list.AsQueryable().Where(orSpec.SatisfiedBy()).ToList();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void UseSpecificationOrOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            var orSpec = leftAdHocSpecification || rightAdHocSpecification;


            //Assert
            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity(identifier) { SampleProperty = "1" };

            var sampleB = new SampleEntity(Guid.NewGuid()) { SampleProperty = "the sample property" };

            list.AddRange(new[] { sampleA, sampleB });

            var result = list.AsQueryable().Where(orSpec.SatisfiedBy()).ToList();

            Assert.True(result.Count == 2);
        }

        //[Fact]
        //public void CreateNotSpecificationithSpecificationTest()
        //{
        //    //Arrange
        //    Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();
        //    var specification = new DirectSpecification<SampleEntity>(specificationCriteria);

        //    //Act
        //    var notSpec = new NotSpecification<SampleEntity>(specification);
        //    var resultCriteria = new PrivateObject(notSpec).GetField("_originalCriteria") as Expression<Func<SampleEntity, bool>>;

        //    //Assert
        //    Assert.NotNull(notSpec);
        //    Assert.NotNull(resultCriteria);
        //    Assert.NotNull(notSpec.SatisfiedBy());
        //}

        //[Fact]
        //public void CreateNotSpecificationWithCriteriaTest()
        //{
        //    //Arrange
        //    Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();

        //    //Act
        //    var notSpec = new NotSpecification<SampleEntity>(specificationCriteria);

        //    //Assert
        //    Assert.NotNull(notSpec);
        //    Assert.NotNull(new PrivateObject(notSpec).GetField("_originalCriteria"));
        //}

        [Fact]
        public void CreateNotSpecificationFromNegationOperator()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();


            //Act
            var spec = new DirectSpecification<SampleEntity>(specificationCriteria);
            ISpecification<SampleEntity> notSpec = !spec;

            //Assert
            Assert.NotNull(notSpec);
        }

        [Fact]
        public void CheckNotSpecificationOperators()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();

            //Act
            Specification<SampleEntity> spec = new DirectSpecification<SampleEntity>(specificationCriteria);
            var notSpec = !spec;
            ISpecification<SampleEntity> resultAnd = notSpec && spec;
            ISpecification<SampleEntity> resultOr = notSpec || spec;

            //Assert
            Assert.NotNull(notSpec);
            Assert.NotNull(resultAnd);
            Assert.NotNull(resultOr);
        }

        [Fact]
        public void CreateNotSpecificationNullSpecificationThrowArgumentNullExceptionTest()
        {
            //Act
            Assert.Throws<ArgumentNullException>(() => new NotSpecification<SampleEntity>((ISpecification<SampleEntity>?)default!));
        }

        [Fact]
        public void CreateNotSpecificationNullCriteriaThrowArgumentNullExceptionTest()
        {
            //Act
            Assert.Throws<ArgumentNullException>(() => new NotSpecification<SampleEntity>((Expression<Func<SampleEntity, bool>>?)default!));
        }

        [Fact]
        public void CreateTrueSpecificationTest()
        {
            //Arrange
            ISpecification<SampleEntity> trueSpec = new TrueSpecification<SampleEntity>();
            const bool expected = true;
            var actual = trueSpec.SatisfiedBy().Compile()(new SampleEntity());

            //Assert
            Assert.NotNull(trueSpec);
            Assert.Equal(expected, actual);
        }
    }
}
