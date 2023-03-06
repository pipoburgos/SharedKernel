using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SharedKernel.Infrastructure.Data.Dapper.Queries
{
    /// <summary> </summary>
    public class CustomDynamicParameters : DynamicParameters
    {
        /// <summary> </summary>
        public CustomDynamicParameters(DynamicParameters dynamicParameters) : base(dynamicParameters) { }

        /// <summary> </summary>
        public CustomDynamicParameters(IEnumerable<KeyValuePair<string, object>> keyValuePairs) : base(keyValuePairs) { }

        /// <summary> </summary>
        public CustomDynamicParameters() { }

        /// <summary> </summary>
        public void AddParameter(string paramName, object value)
        {
            if (value == default) return;

            if (value.GetType().IsAssignableFrom(typeof(DateTime)) ||
                value.GetType().IsAssignableFrom(typeof(DateTime?)))
                Add(paramName, value, DbType.DateTime2);
            else
                Add(paramName, value);
        }

        /// <summary> </summary>
        public static CustomDynamicParameters Create(object obj)
        {
            var parametrosLista = obj
            .GetType()
            .GetProperties()
            .Select(a => new KeyValuePair<string, object>(a.Name, GetProperty(obj, a.Name)))
            .ToList();

            return new CustomDynamicParameters(parametrosLista);
        }

        private static object GetProperty(object obj, string name)
        {
            var propertyInfo = obj.GetType().GetProperty(name);
            return propertyInfo == null || !propertyInfo.CanRead
            ? default
            : propertyInfo.GetValue(obj);
        }

    }
}
