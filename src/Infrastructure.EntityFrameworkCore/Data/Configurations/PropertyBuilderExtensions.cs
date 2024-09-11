using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations;

/// <summary> . </summary>
public static class PropertyBuilderExtensions
{
    /// <summary> . </summary>
    public static PropertyBuilder<TProperty> Currency<TProperty>(this PropertyBuilder<TProperty> propertyBuilder,
        int precision = 18) => propertyBuilder.HasColumnType($"decimal({precision},2)");

    /// <summary> . </summary>
    public static PropertyBuilder<TProperty> OnlyDate<TProperty>(this PropertyBuilder<TProperty> propertyBuilder) =>
        propertyBuilder.HasColumnType("Date");
}