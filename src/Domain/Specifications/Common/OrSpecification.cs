using System;
using System.Linq.Expressions;

namespace SharedKernel.Domain.Specifications.Common
{
    /// <inheritdoc />
    /// <summary>
    /// A Logic OR Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class OrSpecification<T>
         : CompositeSpecification<T>
         where T : class
    {
        #region Members

        private readonly ISpecification<T> _rightSideSpecification;
        private readonly ISpecification<T> _leftSideSpecification;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Default constructor for AndSpecification
        /// </summary>
        /// <param name="leftSide">Left side specification</param>
        /// <param name="rightSide">Right side specification</param>
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            _leftSideSpecification = leftSide ?? throw new ArgumentNullException(nameof(leftSide));
            _rightSideSpecification = rightSide ?? throw new ArgumentNullException(nameof(rightSide));
        }

        #endregion

        #region Composite Specification overrides

        /// <inheritdoc />
        /// <summary>
        /// Left side specification
        /// </summary>
        public override ISpecification<T> LeftSideSpecification => _leftSideSpecification;

        /// <inheritdoc />
        /// <summary>
        /// Right side specification
        /// </summary>
        public override ISpecification<T> RightSideSpecification => _rightSideSpecification;

        /// <inheritdoc />
        /// <summary>
        /// <see cref="T:SharedKernel.Domain.Specifications.Common.ISpecification`1" />
        /// </summary>
        /// <returns><see cref="T:SharedKernel.Domain.Specifications.Common.ISpecification`1" /></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = _leftSideSpecification.SatisfiedBy();
            var right = _rightSideSpecification.SatisfiedBy();

            return left.Or(right);

        }

        #endregion
    }
}
