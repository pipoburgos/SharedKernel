using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharedKernel.Domain.Specifications.Common
{
    /// <inheritdoc />
    /// <summary>
    /// Helper for rebind parameters without use Invoke method in expressions
    /// ( this methods is not supported in all linq query providers,
    /// for example in Linq2Entities is not supported)
    /// </summary>
    public sealed class ParameterRebind : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        /// <inheritdoc />
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="map">Map specification</param>
        public ParameterRebind(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replace parameters in expression with a Map information
        /// </summary>
        /// <param name="map">Map information</param>
        /// <param name="exp">Expression to replace parameters</param>
        /// <returns>Expression with parameters replaced</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebind(map).Visit(exp);
        }

        /// <inheritdoc />
        /// <summary>
        /// Visit pattern method
        /// </summary>
        /// <param name="p">A Parameter expression</param>
        /// <returns>New visited expression</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_map.TryGetValue(p, out var replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }

    }
}
