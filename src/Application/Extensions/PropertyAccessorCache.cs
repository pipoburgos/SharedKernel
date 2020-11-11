using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SharedKernel.Application.Extensions
{
    internal class PropertyAccessorCache<T> where T : class
    {
        public LambdaExpression Get(string propertyName)
        {
            return Generate().TryGetValue(propertyName, out var result) ? result : null;
        }

        private IDictionary<string, LambdaExpression> Generate()
        {
            var storage = new Dictionary<string, LambdaExpression>();

            var t = typeof(T);
            var parameter = Expression.Parameter(t, "p");
            foreach (var property in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var lambdaExpression = Expression.Lambda(propertyAccess, parameter);
                storage[property.Name] = lambdaExpression;
            }

            return storage;
        }
    }
}