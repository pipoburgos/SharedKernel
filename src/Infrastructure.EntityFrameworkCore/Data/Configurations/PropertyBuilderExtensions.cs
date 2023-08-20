using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public static PropertyBuilder<TProperty> Currency<TProperty>(this PropertyBuilder<TProperty> propertyBuilder) =>
            propertyBuilder.HasColumnType("decimal(5,2)");

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public static PropertyBuilder<TProperty> OnlyDate<TProperty>(this PropertyBuilder<TProperty> propertyBuilder) =>
            propertyBuilder.HasColumnType("Date");
    }
}
