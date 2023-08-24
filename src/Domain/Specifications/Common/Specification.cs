namespace SharedKernel.Domain.Specifications.Common
{
    /// <inheritdoc />
    /// <summary>
    /// Represent a Expression Specification
    /// <remarks>
    /// Specification overload operators for create AND,OR or NOT specifications.
    /// Additionally overload AND and OR operators with the same sense of ( binary And and binary Or ).
    /// C# couldn't overload the AND and OR operators directly since the framework doesnt allow such craziness. But
    /// with overloading false and true operators this is possible. For explain this behavior please read
    /// http://msdn.microsoft.com/en-us/library/aa691312(VS.71).aspx
    /// </remarks>
    /// </summary>
    /// <typeparam name="TEntity">Type of item in the criteria</typeparam>
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        #region ISpecification<TEntity> Members

        /// <inheritdoc />
        /// <summary>
        /// IsSatisfied Specification pattern method,
        /// </summary>
        /// <returns>Expression that satisfy this specification</returns>
        public abstract Expression<Func<TEntity, bool>> SatisfiedBy();

        #endregion

        #region Override Operators

        /// <summary>
        ///  And operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this AND operation</param>
        /// <param name="rightSideSpecification">right operand in this AND operation</param>
        /// <returns>New specification</returns>
        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification,
            Specification<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Or operator
        /// </summary>
        /// <param name="leftSideSpecification">left operand in this OR operation</param>
        /// <param name="rightSideSpecification">left operand in this OR operation</param>
        /// <returns>New specification </returns>
        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification,
            Specification<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Not specification
        /// </summary>
        /// <param name="specification">Specification to negate</param>
        /// <returns>New specification</returns>
        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

#pragma warning disable IDE0060

        /// <summary>
        /// Override operator false, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See False operator in C#</returns>
        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }

        /// <summary>
        /// Override operator True, only for support AND OR operators
        /// </summary>
        /// <param name="specification">Specification instance</param>
        /// <returns>See True operator in C#</returns>
        public static bool operator true(Specification<TEntity> specification)
        {
            return false;
        }

        #endregion
    }
}

