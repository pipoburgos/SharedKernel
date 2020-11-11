using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using SharedKernel.Application.Exceptions;

namespace SharedKernel.Application.Reflection
{
    public static class ReflectionHelper
    {
        public static Assembly GetAssemblyByName(string name)
        {
            if (name == null) return null;

            name = name.ToUpper(CultureInfo.InvariantCulture);
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(x => x.FullName.ToUpper(CultureInfo.InvariantCulture).Contains(name));
        }

        public static Type GetType(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .FirstOrDefault(type => type.Name.Equals(name, StringComparison.InvariantCulture));
        }

        public static Type GetType(string assemblyName, string name)
        {
            if (string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(name)) return null;

            var assembly = GetAssemblyByName(assemblyName);

            return GetType(assembly, name);
        }

        public static Type GetType(Assembly assembly, string name)
        {
            if (assembly == null) return null;

            return assembly.GetTypes()
                .FirstOrDefault(type => type.Name.Equals(name, StringComparison.InvariantCulture));
        }

        public static T CreateInstance<T>(Type type)
        {
            var instance = (T)FormatterServices.GetUninitializedObject(type);

            if (instance == null)
                throw new ArgumentNullException(nameof(T));

            return instance;
        }

        public static T CreateInstance<T>()
        {
            var instance = (T)FormatterServices.GetUninitializedObject(typeof(T));

            if (instance == null)
                throw new ArgumentNullException(nameof(T));

            return instance;
        }

        public static void SetProperty<T>(T obj, string name, object value)
        {
            var propertyInfo = typeof(T).GetProperty(name);
            if (propertyInfo == null)
                throw new ArgumentNullException(name);

            if (!propertyInfo.CanWrite)
                throw new ArgumentNullException(
                    $"No existe la propiedad {propertyInfo.Name} con escritura en {typeof(T).Name}");

            var t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
#if !NET40
            if (t == typeof(Guid) || t == typeof(Guid?))
                propertyInfo.SetValue(obj, value == null ? null : (Guid?)new Guid(value.ToString()));
            else
                propertyInfo.SetValue(obj, Convert.ChangeType(value, t));
#else
            if (t == typeof(Guid) || t == typeof(Guid?))
                propertyInfo.SetValue(obj, value == null ? null : (Guid?)new Guid(value.ToString()), new object[0]);
            else
                propertyInfo.SetValue(obj, Convert.ChangeType(value, t), new object[0]);
#endif
        }

        public static void SetField<T>(T obj, string name, object value)
        {
            if (value == default)
                return;

            try
            {
                var fieldInfo = typeof(T).GetField(name);
                if (fieldInfo == null)
                    throw new ArgumentNullException(nameof(name));

                var t = Nullable.GetUnderlyingType(fieldInfo.FieldType) ?? fieldInfo.FieldType;

                if (t == typeof(Guid) || t == typeof(Guid?))
                    fieldInfo.SetValue(obj, new Guid(value.ToString()));
                else
                    fieldInfo.SetValue(obj, Convert.ChangeType(value, t));
            }
            catch (Exception e)
            {
                throw new ApplicationException(ExceptionCodes.REFLEXION_001, e);
            }
        }

        public static TResult GetProperty<T, TResult>(T obj, string name)
        {
            var propertyInfo = typeof(T).GetProperty(name);
#if !NET40
            return propertyInfo == null || !propertyInfo.CanRead
                ? default
                : (TResult)Convert.ChangeType(propertyInfo.GetValue(obj), typeof(TResult));
#else
            return propertyInfo == null || !propertyInfo.CanRead
                ? default
                : (TResult)Convert.ChangeType(propertyInfo.GetValue(obj, new object[0]), typeof(TResult));
#endif
        }
    }
}
