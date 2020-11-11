using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<TProperty> Currency<TProperty>(this PropertyBuilder<TProperty> propertyBuilder) =>
            propertyBuilder.HasColumnType("decimal(5,2)");

        public static PropertyBuilder<TProperty> OnlyDate<TProperty>(this PropertyBuilder<TProperty> propertyBuilder) =>
            propertyBuilder.HasColumnType("Date");
    }
}
