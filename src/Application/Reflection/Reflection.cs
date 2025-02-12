﻿using SharedKernel.Application.Exceptions;
using System.Reflection;

namespace SharedKernel.Application.Reflection;

/// <summary> Reflection helper. </summary>
public static class ReflectionHelper
{
    /// <summary> . </summary>
    public static Assembly? GetAssemblyByName(string name)
    {
        if (name == default!)
            return default!;

        return AppDomain.CurrentDomain
            .GetAssemblies()
#if NET6_0_OR_GREATER
            .FirstOrDefault(x => x.FullName?.Contains(name, StringComparison.InvariantCulture) == true);
#else
            .FirstOrDefault(x => x.FullName.ToUpper(CultureInfo.InvariantCulture).Contains(name.ToUpper(CultureInfo.InvariantCulture)));
#endif
    }

    /// <summary> . </summary>
    public static Type? GetType(string name)
    {
        if (string.IsNullOrEmpty(name))
            return default;

        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .FirstOrDefault(type => type.Name.Equals(name, StringComparison.InvariantCulture));
    }

    /// <summary> . </summary>
    public static Type? GetType(string assemblyName, string name)
    {
        if (string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(name))
            return default;

        var assembly = GetAssemblyByName(assemblyName);

        return GetType(assembly, name);
    }

    /// <summary> . </summary>
    public static Type? GetType(Assembly? assembly, string name)
    {
        if (assembly == null)
            return default!;

        return assembly.GetTypes()
            .FirstOrDefault(type => type.Name.Equals(name, StringComparison.InvariantCulture));
    }

    /// <summary> . </summary>
    public static T CreateInstance<T>(Type type)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(type);
#else
        if (type == default)
            throw new ArgumentNullException(nameof(type));
#endif

#if NET6_0_OR_GREATER
        var instance = (T)RuntimeHelpers.GetUninitializedObject(type);
#else
        var instance = (T)FormatterServices.GetUninitializedObject(type);
#endif

        if (instance == null)
            throw new ArgumentNullException(nameof(T));

        return instance;
    }

    /// <summary> . </summary>
    public static T CreateInstance<T>()
    {
        return CreateInstance<T>(typeof(T));
    }

    /// <summary> . </summary>
    public static void SetProperty<T>(T obj, string name, object? value)
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
        {
            var valueString = value?.ToString();
            if (valueString == null)
                propertyInfo.SetValue(obj, null);
            else
                propertyInfo.SetValue(obj, (Guid?)new Guid(valueString));
        }
        else
        {
            propertyInfo.SetValue(obj, Convert.ChangeType(value, t));
        }
#else
        if (t == typeof(Guid) || t == typeof(Guid?))
        {
            var valueString = value?.ToString();
            if (valueString == null)
                propertyInfo.SetValue(obj, null, []);
            else
                propertyInfo.SetValue(obj, (Guid?)new Guid(valueString), []);
        }
        else
        {
            propertyInfo.SetValue(obj, Convert.ChangeType(value, t), []);
        }
#endif
    }

    /// <summary> . </summary>
    public static void SetField<T>(T obj, string name, object value)
    {
        if (value == default!)
            return;

        try
        {
            var fieldInfo = typeof(T).GetField(name);
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(name));

            var t = Nullable.GetUnderlyingType(fieldInfo.FieldType) ?? fieldInfo.FieldType;

            if (t == typeof(Guid) || t == typeof(Guid?))
            {
                var valueString = value.ToString();
#if NETCOREAPP || NET5_0 || NET6_0
                if (valueString == null)
                    fieldInfo.SetValue(obj, null);
                else
#endif
                fieldInfo.SetValue(obj, new Guid(valueString));
            }
            else
            {
                fieldInfo.SetValue(obj, Convert.ChangeType(value, t));
            }
        }
        catch (Exception e)
        {
            throw new ApplicationException(ExceptionCodes.REFLEXION_001, e);
        }
    }

    /// <summary> . </summary>
    public static TResult? GetProperty<T, TResult>(T obj, string name)
    {
        var propertyInfo = typeof(T).GetProperty(name);
#if !NET40
        return propertyInfo == null || !propertyInfo.CanRead
            ? default
            : (TResult?)Convert.ChangeType(propertyInfo.GetValue(obj), typeof(TResult));
#else
        return propertyInfo == null || !propertyInfo.CanRead
            ? default
            : (TResult?)Convert.ChangeType(propertyInfo.GetValue(obj, []), typeof(TResult));
#endif
    }

    /// <summary> . </summary>
    public static TResult? GetProperty<TResult>(Type type, object obj, string name)
    {
        var propertyInfo = type.GetProperty(name);

#if !NET40
        return propertyInfo == null || !propertyInfo.CanRead ? default :
            propertyInfo.GetValue(obj) == default ? default :
            (TResult?)Convert.ChangeType(propertyInfo.GetValue(obj), typeof(TResult));
#else
        return propertyInfo == null || !propertyInfo.CanRead
            ? default
            : (TResult?)Convert.ChangeType(propertyInfo.GetValue(obj, []), typeof(TResult));
#endif
    }

    /// <summary> . </summary>
    public static string? GetStringValue(this PropertyInfo propertyInfo, Type type, object @object)
    {
        var typeNotNullable = propertyInfo.PropertyType.GetTypeNotNullable();
        if (typeNotNullable == default)
            return default;

        if (typeNotNullable.IsEnum)
        {
#if NETFRAMEWORK || NETSTANDARD2_0
            var x = propertyInfo.PropertyType.GetTypeNotNullable();
            if (x == default!)
                return null;

            var tempPriority =
                Enum.Parse(x,
                    propertyInfo.GetValue(@object, null)?.ToString()!);
            return ((int)tempPriority).ToString();
#else
            Enum.TryParse(typeNotNullable,
                propertyInfo.GetValue(@object, null)?.ToString(), out var tempPriority);
            return tempPriority == default ? default : Convert.ToInt32(tempPriority).ToString();
#endif
        }

        if (propertyInfo.PropertyType == typeof(DateTime))
            return GetProperty<DateTime>(type, @object, propertyInfo.Name).ToString("O");


        if (propertyInfo.PropertyType == typeof(DateTime?))
        {
            var date = GetProperty<DateTime>(type, @object, propertyInfo.Name);

            return date == default ? default : date.ToString("O");
        }

        if (propertyInfo.PropertyType != typeof(string) &&
            typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
        {
            var enumerable = (IEnumerable?)propertyInfo.GetValue(@object, null);

            if (enumerable == default)
                return string.Empty;

            var a = string.Join(",", enumerable.Cast<object>().Select(e => e?.ToString()));

            return a;
        }

        return propertyInfo.GetValue(@object, null)?.ToString();
    }

    /// <summary> . </summary>
    public static Type? GetTypeNotNullable(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
            ? Nullable.GetUnderlyingType(type)
            : type;
    }

    /// <summary> . </summary>
    public static bool HasParameterlessConstructor(this Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        // Buscar un constructor sin parámetros
        var constructor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, // Usar el enlazador predeterminado
            Type.EmptyTypes, // Tipos de parámetros (vacío para sin parámetros)
            null // Modificadores de parámetros (no aplica aquí)
        );

        // Si se encontró un constructor sin parámetros, devolver true
        return constructor != null;
    }
}
